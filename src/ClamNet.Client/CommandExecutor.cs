using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ClamNet.Client.Enums;
using ClamNet.Client.Exceptions;

namespace ClamNet.Client
{
    public class CommandExecutor : ICommandExecutor
    {
        public CommandExecutor(ISocketClient socketClient, string host, int port)
        {
            this.SocketClient = socketClient;
            this.Host = host;
            this.Port = port;
        }

        public ISocketClient SocketClient { get; private set; }

        public string Host { get; private set; }

        public int Port { get; private set; }

        public void Configure(ISocketClient socketClient, string host, int port)
        {
            this.SocketClient = socketClient;
            this.Host = host;
            this.Port = port;
        }

        public async Task<string> Execute(ScanClientCommand command, Func<NetworkStream, Task> additionalCommand = null)
        {
            string result;

            try
            {
                await this.SocketClient.ConnectAsync(this.Host, this.Port);

                var commandString = $"{command}".ToUpperInvariant();

                using (var stream = this.SocketClient.GetStream())
                {
                    var commandBytes = Encoding.UTF8.GetBytes($"z{commandString}\0");
                    await stream.WriteAsync(commandBytes, 0, commandBytes.Length);

                    if (additionalCommand != null)
                    {
                        await additionalCommand(stream);
                    }

                    using (var reader = new StreamReader(stream))
                    {
                        result = await reader.ReadToEndAsync();

                        if (!string.IsNullOrEmpty(result))
                        {
                            result = result.TrimEnd('\0');
                        }
                    }
                }
            }
            catch
            {
                throw new ClamAvServerException(this.Host, this.Port);
            }
            finally
            {
                if (this.SocketClient.Connected)
                {
                    this.SocketClient.Close();
                }
            }

            return result;
        }
    }
}