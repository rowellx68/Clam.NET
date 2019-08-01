namespace ClamNet.Client
{
    public class ScanClientBuilder : IScanClientBuilder, IScanClientConfigured
    {
        public string Host { get; private set; }

        public int Port { get; private set; }

        public ISocketClient SocketClient { get; private set; }

        public ICommandExecutor CommandExecutor { get; private set; }

        public IScanClientBuilder WithServerAddress(string host, int port)
        {
            this.Host = host;
            this.Port = port;

            return this;
        }

        public IScanClientBuilder WithSocketClient(ISocketClient client)
        {
            this.SocketClient = client;

            return this;
        }

        public IScanClientConfigured WithCommandExecutor(ICommandExecutor executor)
        {
            executor.Configure(this.SocketClient, this.Host, this.Port);

            this.CommandExecutor = executor;

            return this;
        }

        public IScanClient Build()
        {
            return new ScanClient(this.CommandExecutor);
        }
    }
}