using System.Text.Json;
using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Converters;

internal sealed class IntentEventConverter : EventConverterBase<IntentEvent>
{
    public override IntentEvent Convert(RawWyomingEvent @event)
    {
        return new IntentEvent
        {
            Data = @event.Data!.Value.Deserialize(WyomingSerializerJsonContext.Default.IntentEventData)!
        };
    }

    public override RawWyomingEvent Convert(IntentEvent @event)
    {
        return new RawWyomingEvent
        {
            Type = Constants.EventTypes.Intent,
            Data = JsonSerializer.SerializeToElement(@event.Data, WyomingSerializerJsonContext.Default.IntentEventData)
        };
    }
}
