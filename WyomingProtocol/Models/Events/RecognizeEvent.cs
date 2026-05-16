using WyomingProtocol.Models.Events.EventData;

namespace WyomingProtocol.Models.Events;

public sealed class RecognizeEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.Recognize;

    public required RecognizeEventData Data { get; init; }
}
