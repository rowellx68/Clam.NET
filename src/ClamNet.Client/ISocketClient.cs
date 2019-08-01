using System.Net.Sockets;
using System.Threading.Tasks;

namespace ClamNet.Client
{
    public interface ISocketClient
    {
        bool Connected { get; }

        Task ConnectAsync(string host, int port);

        NetworkStream GetStream();

        void Close();
    }
}