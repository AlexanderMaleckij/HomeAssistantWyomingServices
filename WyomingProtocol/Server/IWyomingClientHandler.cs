using System.Net.Sockets;

namespace WyomingProtocol.Server;

internal interface IWyomingClientHandler
{
    Task HandleAsync(TcpClient client, CancellationToken cancellationToken);
}
