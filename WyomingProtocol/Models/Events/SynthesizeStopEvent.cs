namespace WyomingProtocol.Models.Events;

/// <summary>
/// End of streaming request to synthesize audio from text.
/// </summary>
public sealed class SynthesizeStopEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.SynthesizeStop;
}
