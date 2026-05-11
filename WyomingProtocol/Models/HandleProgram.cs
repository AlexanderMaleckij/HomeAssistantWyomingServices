namespace WyomingProtocol.Models;

/// <summary>
/// Intent handling service.
/// </summary>
public sealed class HandleProgram : Artifact
{
    /// <summary>
    /// List of available models.
    /// </summary>
    public required HandleModel[] Models { get; init; }

    /// <summary>
    /// true if handled response streaming events are supported.
    /// </summary>
    public bool SupportsHandledStreaming { get; init; }
}
