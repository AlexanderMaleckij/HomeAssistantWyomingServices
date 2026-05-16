namespace WyomingProtocol.Models.Events;

public sealed class SynthesizeChunkEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.SynthesizeChunk;

    public required SynthesizeChunkEventData Data { get; init; }
}

public sealed class SynthesizeChunkEventData
{
    /// <summary>
    /// Chunk of text to synthesize.
    /// </summary>
    public required string Text { get; init; }
}
