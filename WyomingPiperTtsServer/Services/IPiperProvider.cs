namespace WyomingPiperTtsServer.Services;

internal interface IPiperProvider
{
    IPiper GetPiper(string? modelId = null);
}
