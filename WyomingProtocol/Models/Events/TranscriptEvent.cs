using WyomingProtocol.Models.Events.EventData;

namespace WyomingProtocol.Models.Events;

public sealed class TranscriptEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.Transcript;

    public required TranscriptEventData Data { get; init; }
}
