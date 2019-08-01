using System.Threading.Tasks;
using ClamNet.Client.Models;

namespace ClamNet.Client
{
    public interface IScanClient
    {
        ICommandExecutor CommandExecutor { get; }

        Task<string> GetVersion();

        Task<bool> Ping();

        Task<ScanResult> ScanBytes(byte[] data);
    }
}