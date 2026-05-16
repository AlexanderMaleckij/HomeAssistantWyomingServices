using WyomingProtocol.Models.Events.EventData;

namespace WyomingProtocol.Models.Events;

public sealed class IntentEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.Intent;

    public required IntentEventData Data { get; init; }
}
