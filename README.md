# Clam.NET [![Build status](https://ci.appveyor.com/api/projects/status/yh026y9ykdprdwgv?svg=true)](https://ci.appveyor.com/project/rowellx68/clam-net)

A small client for ClamAV server which allows you to scan byte arrays for any malicious content.

## Requirements

A running instance of ClamAV server should be accessible at the host and port provided.

Binaries could be had from here: https://www.clamav.net/downloads

## Example Usage

```csharp
var executor = new CommandExecutor(new SocketClient(), "localhost", 3310);
var client = new ScanClient(executor);

var ping = await client.Ping();
```

```csharp
class SomeClass
{
    public SomeClass(IScanClientBuilder builder, ISocketClient socketClient, ICommandExecutor commandExecutor)
    {
        this.ScanClient = builder
            .WithServerAddress("localhost", 3310)
            .WithSocketClient(socketClient)
            .WithCommandExecutor(commandExecutor)
            .Build();
    }

    private IScanClient ScanClient { get; }
}
```