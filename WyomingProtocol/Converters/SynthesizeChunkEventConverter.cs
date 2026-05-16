using System.Text.Json;
using WyomingProtocol.Converters.Shared;
using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Converters;

internal sealed class SynthesizeChunkEventConverter : EventConverterBase<SynthesizeChunkEvent>
{
    public override SynthesizeChunkEvent Convert(RawWyomingEvent @event)
    {
        return new SynthesizeChunkEvent
        {
            Data = @event.Data!.Value.Deserialize(WyomingSerializerJsonContext.Default.SynthesizeChunkEventData)!
        };
    }

    public override RawWyomingEvent Convert(SynthesizeChunkEvent @event)
    {
        return new RawWyomingEvent
        {
            Type = @event.Type,
            Data = JsonSerializer.SerializeToElement(@event.Data, WyomingSerializerJsonContext.Default.SynthesizeChunkEventData)
        };
    }
}
