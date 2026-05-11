using System.Text.Json;
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
            Type = Constants.EventTypes.SynthesizeStart,
            Data = JsonSerializer.SerializeToElement(@event.Data, WyomingSerializerJsonContext.Default.SynthesizeStartEventData)
        };
    }
}
