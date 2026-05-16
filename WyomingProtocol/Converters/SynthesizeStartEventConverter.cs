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
            Data = @event.Data!.Value.Deserialize(WyomingSerializerJsonContext.Default.SynthesizeStartEventData)!
        };
    }

    public override RawWyomingEvent Convert(SynthesizeStartEvent @event)
    {
        return new RawWyomingEvent
        {
            Type = @event.Type,
            Data = JsonSerializer.SerializeToElement(@event.Data, WyomingSerializerJsonContext.Default.SynthesizeStartEventData)
        };
    }
}
