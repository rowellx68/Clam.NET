namespace ClamNet.Client
{
    public interface IScanClientBuilder
    {
        IScanClientBuilder WithServerAddress(string host, int port);

        IScanClientBuilder WithSocketClient(ISocketClient client);
    }

    public interface IScanClientConfigured
    {
        IScanClientConfigured WithCommandExecutor(ICommandExecutor executor);

        IScanClient Build();
    }
}