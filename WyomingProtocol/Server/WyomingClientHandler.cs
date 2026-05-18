using Microsoft.Extensions.DependencyInjection;
using System.Net.Sockets;
using WyomingProtocol.Services;

namespace WyomingProtocol.Server;

internal sealed class WyomingClientHandler : IWyomingClientHandler
{
    private readonly string _handlerKey;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IWyomingSerializer _wyomingSerializer;

    public WyomingClientHandler(
        string handlerKey,
        IServiceScopeFactory serviceScopeFactory,
        IWyomingSerializer wyomingSerializer)
    {
        ArgumentNullException.ThrowIfNull(handlerKey);
        ArgumentNullException.ThrowIfNull(serviceScopeFactory);
        ArgumentNullException.ThrowIfNull(wyomingSerializer);

        _handlerKey = handlerKey;
        _serviceScopeFactory = serviceScopeFactory;
        _wyomingSerializer = wyomingSerializer;
    }

    public async Task HandleAsync(TcpClient client, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateAsyncScope();
        var handler = scope.ServiceProvider.GetRequiredKeyedService<IWyomingEventHandler>(_handlerKey);

        await WithDisposeAsync(handler, () => HandleInternalAsync(client, handler, cancellationToken));
    }

    private async Task HandleInternalAsync(TcpClient client, IWyomingEventHandler handler, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var wyomingEvent = await _wyomingSerializer.DeserializeAsync(client.GetStream(), cancellationToken);
            var context = new WyomingRequestContext(client, _wyomingSerializer);
            await handler.HandleAsync(wyomingEvent, context, cancellationToken);

            // It is not possible to determine whether the TcpClient connection was closed or not.
            if (context.IsClosedConnection)
            {
                return;
            }
        }
    }

    private async static Task WithDisposeAsync<T>(T potentiallyDisposableObject, Func<Task> action)
    {
        if (potentiallyDisposableObject is IAsyncDisposable asyncDisposableObject)
        {
            await using (asyncDisposableObject)
            {
                await action();
            }
        }
        else if (potentiallyDisposableObject is IDisposable disposableObject)
        {
            using (disposableObject)
            {
                await action();
            }
        }
        else
        {
            await action();
        }
    }
}
