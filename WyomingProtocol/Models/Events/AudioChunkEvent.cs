using WyomingProtocol.Models.Events.EventData;

namespace WyomingProtocol.Models.Events;

public sealed class AudioChunkEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.AudioChunk;

    public required AudioChunkEventData Data { get; init; }

    public required byte[] Payload { get; init; }
}
