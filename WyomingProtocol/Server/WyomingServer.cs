using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace WyomingProtocol.Server;

internal sealed class WyomingServer : IWyomingServer
{
    private readonly IWyomingClientHandler _wyomingClientHandler;
    private readonly ILogger<WyomingServer> _logger;

    public WyomingServer(
        IWyomingClientHandler wyomingClientHandler,
        ILogger<WyomingServer> logger)
    {
        ArgumentNullException.ThrowIfNull(wyomingClientHandler);
        ArgumentNullException.ThrowIfNull(logger);

        _wyomingClientHandler = wyomingClientHandler;
        _logger = logger;
    }

    public async Task RunAsync(IPAddress host, int port, CancellationToken cancellationToken)
    {
        var listener = new TcpListener(host, port);
        listener.Start();

        _logger.LogInformation("Server started on {host}:{port}", host, port);

        var activeTasks = new ConcurrentDictionary<Guid, Task>();

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var client = await listener.AcceptTcpClientAsync(cancellationToken);
                    var id = Guid.NewGuid();

                    var clientTask = Task.Run(() => HandleClientAsync(client, cancellationToken), cancellationToken)
                        .ContinueWith(_ => activeTasks.TryRemove(id, out Task? _), TaskContinuationOptions.ExecuteSynchronously);

                    activeTasks.TryAdd(id, clientTask);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error accepting client");
                }
            }
        }
        finally
        {
            listener.Stop();

            _logger.LogInformation("Server stopped, waiting for {count} client(s) to finish", activeTasks.Count);

            try
            {
                await Task.WhenAll(activeTasks.Values);
            }
            catch
            {
                // Errors will be logged in the HandleClientAsync.
            }

            _logger.LogInformation("All clients disconnected");
        }
    }

    private async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
    {
        using (client)
        {
            var clientEndpoint = client.Client.RemoteEndPoint;

            _logger.LogInformation("Client connected: {endpoint}", clientEndpoint);

            try
            {
                await _wyomingClientHandler.HandleAsync(client, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Client handler error: {endpoint}", clientEndpoint);
            }
            finally
            {
                _logger.LogInformation("Client disconnected: {endpoint}", clientEndpoint);
            }
        }
    }
}
