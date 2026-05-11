using Microsoft.Extensions.Options;
using WyomingPiperTtsServer.Models.Options;

namespace WyomingPiperTtsServer.Services;

internal sealed class PiperProvider : IPiperProvider
{
    private readonly IOptions<PiperOptions> _piperOptions;

    public PiperProvider(IOptions<PiperOptions> piperOptions)
    {
        ArgumentNullException.ThrowIfNull(piperOptions);

        _piperOptions = piperOptions;
    }

    public IPiper GetPiper(string? modelId = null)
    {
        var options = _piperOptions.Value;

        var model = modelId is null
            ? options.Models.First()
            : options.Models.First(x => x.Id == modelId);

        return new Piper(model.Path, model.Path + ".json", options.EspeakDataDirectory, options.EspeakDllPath);
    }
}
