namespace WyomingProtocol.Models.Events;

public sealed class HandledStopEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.HandledStop;
}
