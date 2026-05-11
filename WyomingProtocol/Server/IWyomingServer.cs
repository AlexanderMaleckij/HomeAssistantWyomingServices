using System.Net;

namespace WyomingProtocol.Server;

public interface IWyomingServer
{
    Task RunAsync(IPAddress host, int port, CancellationToken cancellationToken);
}
