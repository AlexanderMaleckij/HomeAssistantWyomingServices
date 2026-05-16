namespace WyomingProtocol.Models.Events;

internal sealed class IntentsStopEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.IntentsStop;
}
