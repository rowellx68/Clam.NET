using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using ClamNet.Client.Enums;

namespace ClamNet.Client
{
    public interface ICommandExecutor
    {
        ISocketClient SocketClient { get; }

        void Configure(ISocketClient socketClient, string host, int port);

        Task<string> Execute(ScanClientCommand command, Func<NetworkStream, Task> additionalCommand = null);
    }
}