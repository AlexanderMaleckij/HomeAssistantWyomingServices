namespace WyomingProtocol.Models;

/// <summary>
/// Text-to-speech model.
/// </summary>
public sealed class TtsVoice : Artifact
{
    /// <summary>
    /// List of languages supported by the model.
    /// </summary>
    public required string[] Languages { get; init; }

    /// <summary>
    /// List of individual speakers in the model.
    /// </summary>
    public TtsModelSpeaker[]? Speakers { get; init; }
}
