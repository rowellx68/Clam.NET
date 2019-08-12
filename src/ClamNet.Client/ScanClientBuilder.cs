namespace ClamNet.Client
{
    public class ScanClientBuilder : IScanClientBuilder
    {
        public ScanClientBuilder(ISocketClient socketClient, ICommandExecutor commandExecutor)
        {
            this.SocketClient = socketClient;
            this.CommandExecutor = commandExecutor;
        }

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

        public IScanClientBuilder WithDefaults()
        {
            this.Host = "localhost";
            this.Port = 3310;

            this.CommandExecutor.Configure(this.SocketClient, this.Host, this.Port);

            return this;
        }

        public IScanClientBuilder WithCommandExecutor(ICommandExecutor executor)
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