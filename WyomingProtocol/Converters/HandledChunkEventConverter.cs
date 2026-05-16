using System.Text.Json;
using WyomingProtocol.Converters.Shared;
using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Converters;

internal sealed class HandledChunkEventConverter : EventConverterBase<HandledChunkEvent>
{
    public override HandledChunkEvent Convert(RawWyomingEvent @event)
    {
        return new HandledChunkEvent
        {
            Data = @event.Data!.Value.Deserialize(WyomingSerializerJsonContext.Default.HandledChunkEventData)!
        };
    }

    public override RawWyomingEvent Convert(HandledChunkEvent @event)
    {
        return new RawWyomingEvent
        {
            Type = @event.Type,
            Data = JsonSerializer.SerializeToElement(@event.Data, WyomingSerializerJsonContext.Default.HandledChunkEventData)
        };
    }
}
