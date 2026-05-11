using WyomingProtocol.Models.Events.EventData;

namespace WyomingProtocol.Models.Events;

public sealed class AudioStartEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.AudioStart;

    public required AudioStartEventData Data { get; init; }
}
