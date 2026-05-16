using System.Text.Json;
using WyomingProtocol.Converters.Shared;
using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Converters;

internal sealed class AudioChunkEventConverter : EventConverterBase<AudioChunkEvent>
{
    public override AudioChunkEvent Convert(RawWyomingEvent @event)
    {
        return new AudioChunkEvent
        {
            Data = @event.Data!.Value.Deserialize(WyomingSerializerJsonContext.Default.AudioChunkEventData)!,
            Payload = @event.Payload!
        };
    }

    public override RawWyomingEvent Convert(AudioChunkEvent @event)
    {
        return new RawWyomingEvent
        {
            Type = @event.Type,
            Data = JsonSerializer.SerializeToElement(@event.Data, WyomingSerializerJsonContext.Default.AudioChunkEventData),
            Payload = @event.Payload
        };
    }
}
