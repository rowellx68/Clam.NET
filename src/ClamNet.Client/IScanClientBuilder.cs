namespace ClamNet.Client
{
    public interface IScanClientBuilder
    {
        IScanClientBuilder WithServerAddress(string host, int port);

        IScanClientBuilder WithSocketClient(ISocketClient client);

        IScanClientBuilder WithDefaults();

        IScanClientBuilder WithCommandExecutor(ICommandExecutor executor);

        IScanClient Build();
    }
}