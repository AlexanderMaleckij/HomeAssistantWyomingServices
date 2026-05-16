using System.Text.Json;

namespace WyomingProtocol.Models.Events;

public sealed class TranscriptEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.Transcript;

    public required TranscriptEventData Data { get; init; }
}

/// <summary>
/// Transcription response from ASR system.
/// </summary>
public sealed class TranscriptEventData
{
    /// <summary>
    /// Text transcription of spoken audio.
    /// </summary>
    public required string Text { get; init; }

    /// <summary>
    /// Context for next interaction.
    /// </summary>
    public Dictionary<string, JsonElement>? Context { get; init; }

    /// <summary>
    /// Language of the text.
    /// </summary>
    public string? Language { get; init; }
}
