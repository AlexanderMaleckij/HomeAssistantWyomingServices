using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WyomingProtocol.Services;

namespace WyomingProtocol.Server;

internal sealed class WyomingServerFactory : IWyomingServerFactory
{
    private readonly IWyomingSerializer _wyomingSerializer;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<WyomingServer> _serverLogger;

    public WyomingServerFactory(
        IWyomingSerializer wyomingSerializer,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<WyomingServer> serverLogger)
    {
        ArgumentNullException.ThrowIfNull(wyomingSerializer);
        ArgumentNullException.ThrowIfNull(serviceScopeFactory);
        ArgumentNullException.ThrowIfNull(serverLogger);

        _wyomingSerializer = wyomingSerializer;
        _serviceScopeFactory = serviceScopeFactory;
        _serverLogger = serverLogger;
    }

    public IWyomingServer Create(string handlerKey)
    {
        ArgumentException.ThrowIfNullOrEmpty(handlerKey);

        var clientHandler = new WyomingClientHandler(handlerKey, _serviceScopeFactory, _wyomingSerializer);

        return new WyomingServer(clientHandler, _serverLogger);
    }
}
