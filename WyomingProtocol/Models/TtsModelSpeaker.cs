namespace WyomingProtocol.Models;

/// <summary>
/// Individual speaker in a multi-speaker voice.
/// </summary>
public sealed class TtsModelSpeaker
{
    /// <summary>
    /// Unique name of the speaker.
    /// </summary>
    public required string Name { get; init; }
}
