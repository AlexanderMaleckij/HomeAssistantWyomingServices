using Microsoft.Extensions.Options;
using System.Net;
using WyomingPiperTtsServer.Models.Options;
using WyomingProtocol.Server;

namespace WyomingPiperTtsServer.Services;

internal sealed class WyomingAsrTtsServer : IWyomingAsrTtsServer
{
    private readonly IWyomingServerFactory _wyomingServerFactory;
    private readonly IOptions<WyomingPiperTtsServerOptions> _serverOptions;

    public WyomingAsrTtsServer(
        IWyomingServerFactory wyomingServerFactory,
        IOptions<WyomingPiperTtsServerOptions> serverOptions)
    {
        ArgumentNullException.ThrowIfNull(wyomingServerFactory);
        ArgumentNullException.ThrowIfNull(serverOptions);

        _wyomingServerFactory = wyomingServerFactory;
        _serverOptions = serverOptions;
    }

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        var serverOptions = _serverOptions.Value;
        var server = _wyomingServerFactory.Create(Constants.WyomingHandlers.TTS);

        return server.RunAsync(IPAddress.Parse(serverOptions.Host), serverOptions.Port, cancellationToken);
    }
}
