namespace WyomingProtocol.Models;

/// <summary>
/// Text-to-speech service.
/// </summary>
public sealed class TtsProgram : Artifact
{
    /// <summary>
    /// List of available voices.
    /// </summary>
    public required TtsVoice[] Voices { get; init; }

    /// <summary>
    /// true if program can stream text chunks.
    /// </summary>
    public bool SupportsSynthesizeStreaming { get; init; }
}
