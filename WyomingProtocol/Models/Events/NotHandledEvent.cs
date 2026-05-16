using WyomingProtocol.Models.Events.EventData;

namespace WyomingProtocol.Models.Events;

internal sealed class NotHandledEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.NotHandled;

    public required NotHandledEventData Data { get; init; }
}
