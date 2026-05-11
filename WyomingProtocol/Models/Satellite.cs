namespace WyomingProtocol.Models;

/// <summary>
/// Satellite information.
/// </summary>
public sealed class Satellite : Artifact
{
    /// <summary>
    /// Name of the area the satellite is in.
    /// </summary>
    public string? Area { get; init; }

    /// <summary>
    /// true if a local VAD will be used to detect the end of voice commands.
    /// </summary>
    public bool? HasVad { get; init; }

    /// <summary>
    /// Wake words that are currently being listened for.
    /// </summary>
    public string[]? ActiveWakeWords { get; init; }

    /// <summary>
    /// Maximum number of local wake words that can be run simultaneously.
    /// </summary>
    public int? MaxActiveWakeWords { get; init; }

    /// <summary>
    /// Satellite supports remotely triggering pipeline runs.
    /// </summary>
    public bool? SupportsTrigger { get; init; }
}
