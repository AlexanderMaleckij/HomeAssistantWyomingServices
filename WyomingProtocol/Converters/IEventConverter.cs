using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Converters;

// Non-generic version is required for Native AOT compatible implementation.
// Used in the EventConverterProvider and WyomingSerializer.
public interface IEventConverter
{
    IWyomingEvent Convert(RawWyomingEvent @event);

    RawWyomingEvent Convert(IWyomingEvent @event);
}

public interface IEventConverter<TEvent> : IEventConverter
    where TEvent : IWyomingEvent
{
    new TEvent Convert(RawWyomingEvent @event);

    RawWyomingEvent Convert(TEvent @event);
}
