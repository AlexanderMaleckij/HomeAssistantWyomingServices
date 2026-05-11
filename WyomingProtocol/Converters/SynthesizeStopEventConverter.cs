using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Converters;

internal sealed class SynthesizeStopEventConverter : EventConverterBase<SynthesizeStopEvent>
{
    public override SynthesizeStopEvent Convert(RawWyomingEvent @event)
    {
        return new SynthesizeStopEvent();
    }

    public override RawWyomingEvent Convert(SynthesizeStopEvent @event)
    {
        return new RawWyomingEvent
        {
            Type = Constants.EventTypes.SynthesizeStop
        };
    }
}
