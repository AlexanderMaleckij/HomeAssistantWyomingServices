using System.Text.Json;
using WyomingProtocol.Converters.Shared;
using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Converters;

internal sealed class SynthesizeStartEventConverter : EventConverterBase<SynthesizeStartEvent>
{
    public override SynthesizeStartEvent Convert(RawWyomingEvent @event)
    {
        return new SynthesizeStartEvent
        {
            Data = @event.Data?.Deserialize(WyomingSerializerJsonContext.Default.SynthesizeStartEventData)
        };
    }

    public override RawWyomingEvent Convert(SynthesizeStartEvent @event)
    {
        return new RawWyomingEvent
        {
            Type = @event.Type,
            Data = @event.Data is not null
                ? JsonSerializer.SerializeToElement(@event.Data, WyomingSerializerJsonContext.Default.SynthesizeStartEventData)
                : null
        };
    }
}
