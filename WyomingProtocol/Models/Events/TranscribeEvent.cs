using System.Text.Json;

namespace WyomingProtocol.Models.Events;

public sealed class TranscribeEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.Transcribe;

    public required TranscribeEventData Data { get; init; }
}

/// <summary>
/// Transcription request to ASR system.
/// </summary>
/// <remarks>
/// Followed by AudioStart, AudioChunk+, AudioStop.
/// </remarks>
public sealed class TranscribeEventData
{
    /// <summary>
    /// Name of ASR model to use.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Language of spoken audio to follow.
    /// </summary>
    public string? Language { get; init; }

    /// <summary>
    /// Context from previous interactions.
    /// </summary>
    public Dictionary<string, JsonElement>? Context { get; init; }
}
