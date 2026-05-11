namespace WyomingProtocol.Models.Events.EventData;

public sealed class SynthesizeEventData
{
    /// <summary>
    ///  Text to speak.
    /// </summary>
    public required string Text { get; init; }

    public SynthesizeEventVoice? Voice { get; init; }
}

public sealed class SynthesizeEventVoice
{
    /// <summary>
    /// Name of the voice.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Language of the voice.
    /// </summary>
    public string? Language { get; init; }

    /// <summary>
    /// Speaker of the voice.
    /// </summary>
    public string? Speaker { get; init; }
}
