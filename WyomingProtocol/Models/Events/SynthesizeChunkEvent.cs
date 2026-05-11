using WyomingProtocol.Models.Events.EventData;

namespace WyomingProtocol.Models.Events;

public sealed class SynthesizeChunkEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.SynthesizeChunk;

    public required SynthesizeChunkEventData Data { get; init; }
}
