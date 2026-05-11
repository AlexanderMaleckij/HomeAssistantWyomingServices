namespace WyomingPiperTtsServer.Services;

internal interface IWyomingAsrTtsServer
{
    Task StartAsync(CancellationToken cancellationToken = default);
}
