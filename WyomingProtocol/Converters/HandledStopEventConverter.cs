using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Converters;

internal sealed class HandledStopEventConverter : EventConverterBase<HandledStopEvent>
{
    public override HandledStopEvent Convert(RawWyomingEvent @event)
    {
        return new HandledStopEvent();
    }

    public override RawWyomingEvent Convert(HandledStopEvent @event)
    {
        return new RawWyomingEvent { Type = Constants.EventTypes.HandledStop };
    }
}
