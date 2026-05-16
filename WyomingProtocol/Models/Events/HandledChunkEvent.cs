namespace WyomingProtocol.Models.Events;

public sealed class HandledChunkEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.HandledChunk;

    public required HandledChunkEventData Data { get; init; }
}

public sealed class HandledChunkEventData
{
    /// <summary>
    /// Part of response.
    /// </summary>
    public required string Text { get; init; }
}
