using Microsoft.Extensions.Hosting;

namespace WyomingCrispAsrServer.Services;

internal sealed class BootstrapBackgroundService : BackgroundService
{
    private readonly IWyomingAsrServer _wyomingAsrServer;

    public BootstrapBackgroundService(IWyomingAsrServer wyomingAsrServer)
    {
        ArgumentNullException.ThrowIfNull(wyomingAsrServer);

        _wyomingAsrServer = wyomingAsrServer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _wyomingAsrServer.StartAsync(stoppingToken);
    }
}
