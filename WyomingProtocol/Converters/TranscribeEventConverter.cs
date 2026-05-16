using System.Text.Json;
using WyomingProtocol.Converters.Shared;
using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Converters;

internal sealed class TranscribeEventConverter : EventConverterBase<TranscribeEvent>
{
    public override TranscribeEvent Convert(RawWyomingEvent @event)
    {
        return new TranscribeEvent
        {
            Data = @event.Data!.Value.Deserialize(WyomingSerializerJsonContext.Default.TranscribeEventData)!
        };
    }

    public override RawWyomingEvent Convert(TranscribeEvent @event)
    {
        return new RawWyomingEvent
        {
            Type = @event.Type,
            Data = JsonSerializer.SerializeToElement(@event.Data, WyomingSerializerJsonContext.Default.TranscribeEventData)
        };
    }
}
