using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Converters.Shared;

public abstract class EventConverterBase<TEvent> : IEventConverter<TEvent>
    where TEvent : IWyomingEvent
{
    public abstract TEvent Convert(RawWyomingEvent @event);

    public abstract RawWyomingEvent Convert(TEvent @event);

    RawWyomingEvent IEventConverter.Convert(IWyomingEvent @event) =>
        Convert((TEvent)@event);

    IWyomingEvent IEventConverter.Convert(RawWyomingEvent @event) =>
        Convert(@event);
}
