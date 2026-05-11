using WyomingProtocol.Models.Events.EventData;

namespace WyomingProtocol.Models.Events;

public sealed class TranscribeEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.Transcribe;

    public required TranscribeEventData Data { get; init; }
}
