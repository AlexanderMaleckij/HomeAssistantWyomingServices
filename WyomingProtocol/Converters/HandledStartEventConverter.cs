using System.Text.Json;
using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Converters;

internal sealed class HandledStartEventConverter : EventConverterBase<HandledStartEvent>
{
    public override HandledStartEvent Convert(RawWyomingEvent @event)
    {
        return new HandledStartEvent
        {
            Data = @event.Data!.Value.Deserialize(WyomingSerializerJsonContext.Default.HandledStartEventData)!
        };
    }

    public override RawWyomingEvent Convert(HandledStartEvent @event)
    {
        return new RawWyomingEvent
        {
            Type = Constants.EventTypes.HandledStart,
            Data = JsonSerializer.SerializeToElement(@event.Data, WyomingSerializerJsonContext.Default.HandledStartEventData)
        };
    }
}
