namespace WyomingProtocol.Models.Events.EventData;

public sealed class SynthesizeChunkEventData
{
    /// <summary>
    /// Chunk of text to synthesize.
    /// </summary>
    public required string Text { get; init; }
}
