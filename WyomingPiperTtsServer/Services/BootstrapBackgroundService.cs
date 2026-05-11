using Microsoft.Extensions.Hosting;

namespace WyomingPiperTtsServer.Services;

internal sealed class BootstrapBackgroundService : BackgroundService
{
    private readonly IWyomingAsrTtsServer _wyomingAsrTtsServer;

    public BootstrapBackgroundService(IWyomingAsrTtsServer wyomingAsrTtsServer)
    {
        ArgumentNullException.ThrowIfNull(wyomingAsrTtsServer);

        _wyomingAsrTtsServer = wyomingAsrTtsServer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _wyomingAsrTtsServer.StartAsync(stoppingToken);
    }
}
