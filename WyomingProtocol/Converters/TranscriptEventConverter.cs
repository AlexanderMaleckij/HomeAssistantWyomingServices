using System.Text.Json;
using WyomingProtocol.Converters.Shared;
using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Converters;

internal sealed class TranscriptEventConverter : EventConverterBase<TranscriptEvent>
{
    public override TranscriptEvent Convert(RawWyomingEvent @event)
    {
        return new TranscriptEvent
        {
            Data = @event.Data!.Value.Deserialize(WyomingSerializerJsonContext.Default.TranscriptEventData)!
        };
    }

    public override RawWyomingEvent Convert(TranscriptEvent @event)
    {
        return new RawWyomingEvent
        {
            Type = @event.Type,
            Data = JsonSerializer.SerializeToElement(@event.Data, WyomingSerializerJsonContext.Default.TranscriptEventData)
        };
    }
}
