using WyomingProtocol.Models.Events.EventData;

namespace WyomingProtocol.Models.Events;

internal sealed class HandledChunkEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.HandledChunk;

    public required HandledChunkEventData Data { get; init; }
}
