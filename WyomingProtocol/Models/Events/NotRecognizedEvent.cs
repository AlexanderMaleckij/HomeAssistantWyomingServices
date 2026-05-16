using WyomingProtocol.Models.Events.EventData;

namespace WyomingProtocol.Models.Events;

public sealed class NotRecognizedEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.NotRecognized;

    public required NotRecognizedEventData Data { get; init; }
}
