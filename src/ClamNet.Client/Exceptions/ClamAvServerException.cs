using System;

namespace ClamNet.Client.Exceptions
{
    public class ClamAvServerException : Exception
    {
        public ClamAvServerException(string host, int port) : base($"The ClamAV server is not accessible at {host}:{port}")
        {
        }
    }
}