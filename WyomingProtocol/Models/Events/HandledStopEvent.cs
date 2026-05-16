namespace WyomingProtocol.Models.Events;

internal sealed class HandledStopEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.HandledStop;
}
