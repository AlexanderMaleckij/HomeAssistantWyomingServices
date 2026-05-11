using Microsoft.Extensions.Options;
using System.Net;
using WyomingCrispAsrServer.Models.Options.WyomingAsrServer;
using WyomingProtocol.Server;

namespace WyomingCrispAsrServer.Services;

internal sealed class WyomingAsrServer : IWyomingAsrServer
{
    private readonly IWyomingServerFactory _wyomingServerFactory;
    private readonly IOptions<WyomingAsrServerOptions> _serverOptions;

    public WyomingAsrServer(
        IWyomingServerFactory wyomingServerFactory,
        IOptions<WyomingAsrServerOptions> serverOptions)
    {
        ArgumentNullException.ThrowIfNull(wyomingServerFactory);
        ArgumentNullException.ThrowIfNull(serverOptions);

        _wyomingServerFactory = wyomingServerFactory;
        _serverOptions = serverOptions;
    }

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        var serverOptions = _serverOptions.Value;
        var server = _wyomingServerFactory.Create(Constants.WyomingHandlers.ASR);

        return server.RunAsync(IPAddress.Parse(serverOptions.Host), serverOptions.Port, cancellationToken);
    }
}
