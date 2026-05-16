using WyomingProtocol.Models.Events.EventData;

namespace WyomingProtocol.Models.Events;

public sealed class IntentsStartEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.IntentsStart;

    public required IntentsStartEventData Data { get; init; }
}
