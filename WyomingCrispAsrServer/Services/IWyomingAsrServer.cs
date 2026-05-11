namespace WyomingCrispAsrServer.Services;

internal interface IWyomingAsrServer
{
    Task StartAsync(CancellationToken cancellationToken = default);
}
