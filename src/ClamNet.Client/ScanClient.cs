using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ClamNet.Client.Enums;
using ClamNet.Client.Exceptions;
using ClamNet.Client.Mappers;
using ClamNet.Client.Models;

namespace ClamNet.Client
{
    public class ScanClient : IScanClient
    {
        public ScanClient(ICommandExecutor commandExecutor)
        {
            this.CommandExecutor = commandExecutor;
        }

        public ICommandExecutor CommandExecutor { get; }

        /// <summary>
        /// Defaults to 128KB.
        /// </summary>
        public int MaxChunkSize { get; } = 131072;

        /// <summary>
        /// Defaults to 25MB.
        /// </summary>
        public long MaxStreamSize { get; } = 26214400;

        /// <summary>
        /// Get the current version of the ClamAV server.
        /// </summary>
        /// <returns></returns>
        public Task<string> GetVersion()
        {
            return this.CommandExecutor.Execute(ScanClientCommand.Version);
        }

        /// <summary>
        /// Pings the ClamAV server.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Ping()
        {
            var response = await this.CommandExecutor.Execute(ScanClientCommand.Ping);

            return response.Equals("PONG", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Scans the byte array for potential malicious content.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<ScanResult> ScanBytes(byte[] data)
        {
            using (var sourceStream = new MemoryStream(data))
            {
                var result = await this.CommandExecutor.Execute(ScanClientCommand.InStream,
                    (stream) => this.SendStreamFileChunks(sourceStream, stream));
                var rawResult = new RawScanResult(result);

                return rawResult.ToScanResult();
            }
        }

        private async Task SendStreamFileChunks(Stream sourceStream, Stream clamStream)
        {
            var size = this.MaxChunkSize;
            var bytes = new byte[size];

            while ((size = await sourceStream.ReadAsync(bytes, 0, size)) > 0)
            {
                if (sourceStream.Position > this.MaxStreamSize)
                {
                    throw new MaximumStreamSizeException(this.MaxStreamSize);
                }

                var sizeBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(size));

                await clamStream.WriteAsync(sizeBytes, 0, sizeBytes.Length);
                await clamStream.WriteAsync(bytes, 0, size);
            }

            var end = BitConverter.GetBytes(0);
            await clamStream.WriteAsync(end, 0, end.Length);
        }
    }
}