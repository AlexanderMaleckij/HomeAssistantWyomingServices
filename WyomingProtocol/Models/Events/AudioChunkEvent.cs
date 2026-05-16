namespace WyomingProtocol.Models.Events;

public sealed class AudioChunkEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.AudioChunk;

    public required AudioChunkEventData Data { get; init; }

    public required byte[] Payload { get; init; }
}

public sealed class AudioChunkEventData : AudioFormat
{
    /// <summary>
    /// Milliseconds
    /// </summary>
    public int? Timestamp { get; init; }
}
