namespace WyomingProtocol.Models.Events;

public sealed class IntentsStopEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.IntentsStop;
}
