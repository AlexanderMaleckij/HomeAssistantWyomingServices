using System.Text.Json;
using WyomingProtocol.Converters.Shared;
using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Converters;

internal sealed class NotHandledEventConverter : EventConverterBase<NotHandledEvent>
{
    public override NotHandledEvent Convert(RawWyomingEvent @event)
    {
        return new NotHandledEvent
        {
            Data = @event.Data!.Value.Deserialize(WyomingSerializerJsonContext.Default.NotHandledEventData)!
        };
    }

    public override RawWyomingEvent Convert(NotHandledEvent @event)
    {
        return new RawWyomingEvent
        {
            Type = @event.Type,
            Data = JsonSerializer.SerializeToElement(@event.Data, WyomingSerializerJsonContext.Default.NotHandledEventData)
        };
    }
}
