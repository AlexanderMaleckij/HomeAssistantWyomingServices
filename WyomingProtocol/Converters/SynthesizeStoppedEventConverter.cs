using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Converters;

internal sealed class SynthesizeStoppedEventConverter : EventConverterBase<SynthesizeStoppedEvent>
{
    public override SynthesizeStoppedEvent Convert(RawWyomingEvent @event)
    {
        return new SynthesizeStoppedEvent();
    }

    public override RawWyomingEvent Convert(SynthesizeStoppedEvent @event)
    {
        return new RawWyomingEvent
        {
            Type = Constants.EventTypes.SynthesizeStopped
        };
    }
}
