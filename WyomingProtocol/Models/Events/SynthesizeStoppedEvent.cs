namespace WyomingProtocol.Models.Events;

/// <summary>
/// End of streaming response to streaming request.
/// </summary>
public sealed class SynthesizeStoppedEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.SynthesizeStopped;
}
