using WyomingProtocol.Models.Events.EventData;

namespace WyomingProtocol.Models.Events
{
    public sealed class InfoEvent : IWyomingEvent
    {
        public string Type => Constants.EventTypes.Info;

        public required InfoEventData Data { get; init; }
    }
}
