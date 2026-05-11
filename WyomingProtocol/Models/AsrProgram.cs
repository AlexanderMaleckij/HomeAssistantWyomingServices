namespace WyomingProtocol.Models;

/// <summary>
/// Speech-to-text service.
/// </summary>
public sealed class AsrProgram : Artifact
{
    /// <summary>
    /// List of available models.
    /// </summary>
    public required AsrModel[] Models { get; init; }

    /// <summary>
    ///  true if program can stream transcript chunks.
    /// </summary>
    public bool SupportsTranscriptStreaming { get; init; }
}
