using System.Text.Json;
using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Converters;

internal sealed class IntentsStartEventConverter : EventConverterBase<IntentsStartEvent>
{
    public override IntentsStartEvent Convert(RawWyomingEvent @event)
    {
        return new IntentsStartEvent
        {
            Data = @event.Data!.Value.Deserialize(WyomingSerializerJsonContext.Default.IntentsStartEventData)!
        };
    }

    public override RawWyomingEvent Convert(IntentsStartEvent @event)
    {
        return new RawWyomingEvent
        {
            Type = Constants.EventTypes.IntentsStart,
            Data = JsonSerializer.SerializeToElement(@event.Data, WyomingSerializerJsonContext.Default.IntentsStartEventData),
        };
    }
}
