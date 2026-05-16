using WyomingProtocol.Models.Events.EventData;

namespace WyomingProtocol.Models.Events;

internal sealed class HandledStartEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.HandledStart;

    public required HandledStartEventData Data { get; init; }
}
