using WyomingProtocol.Models.Events.EventData;

namespace WyomingProtocol.Models.Events;

public sealed class SynthesizeStartEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.SynthesizeStart;

    public required SynthesizeStartEventData Data { get; init; }
}
