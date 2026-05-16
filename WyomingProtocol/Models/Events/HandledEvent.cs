using WyomingProtocol.Models.Events.EventData;

namespace WyomingProtocol.Models.Events;

internal sealed class HandledEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.Handled;

    public required HandledEventData Data { get; init; }
}
