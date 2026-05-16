using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Converters;

internal sealed class IntentsStopEventConverter : EventConverterBase<IntentsStopEvent>
{
    public override IntentsStopEvent Convert(RawWyomingEvent @event)
    {
        return new IntentsStopEvent();
    }

    public override RawWyomingEvent Convert(IntentsStopEvent @event)
    {
        return new RawWyomingEvent { Type = Constants.EventTypes.IntentsStop };
    }
}
