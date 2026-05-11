using WyomingProtocol.Models.Events.EventData;

namespace WyomingProtocol.Models.Events;

public sealed class SynthesizeEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.Synthesize;

    public required SynthesizeEventData Data { get; init; }
}
