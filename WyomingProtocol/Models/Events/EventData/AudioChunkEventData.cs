namespace WyomingProtocol.Models.Events.EventData;

public sealed class AudioChunkEventData : AudioFormat
{
    /// <summary>
    /// Milliseconds
    /// </summary>
    public int? Timestamp { get; init; }
}
