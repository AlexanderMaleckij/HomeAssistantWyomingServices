using System.Text.Json;
using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Converters;

internal sealed class HandledEventConverter : EventConverterBase<HandledEvent>
{
    public override HandledEvent Convert(RawWyomingEvent @event)
    {
        return new HandledEvent
        {
            Data = @event.Data!.Value.Deserialize(WyomingSerializerJsonContext.Default.HandledEventData)!,
        };
    }

    public override RawWyomingEvent Convert(HandledEvent @event)
    {
        return new RawWyomingEvent
        {
            Type = Constants.EventTypes.Handled,
            Data = JsonSerializer.SerializeToElement(@event.Data, WyomingSerializerJsonContext.Default.HandledEventData)
        };
    }
}
