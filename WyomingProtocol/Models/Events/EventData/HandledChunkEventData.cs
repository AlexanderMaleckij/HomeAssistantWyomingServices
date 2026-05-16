namespace WyomingProtocol.Models.Events.EventData;

public sealed class HandledChunkEventData
{
    /// <summary>
    /// Part of response.
    /// </summary>
    public required string Text { get; init; }
}
