using System.Text.Json;
using WyomingProtocol.Converters.Shared;
using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Converters;

internal sealed class NotRecognizedEventConverter : EventConverterBase<NotRecognizedEvent>
{
    public override NotRecognizedEvent Convert(RawWyomingEvent @event)
    {
        return new NotRecognizedEvent
        {
            Data = @event.Data!.Value.Deserialize(WyomingSerializerJsonContext.Default.NotRecognizedEventData)!
        };
    }

    public override RawWyomingEvent Convert(NotRecognizedEvent @event)
    {
        return new RawWyomingEvent
        {
            Type = @event.Type,
            Data = JsonSerializer.SerializeToElement(@event.Data, WyomingSerializerJsonContext.Default.NotRecognizedEventData)
        };
    }
}
