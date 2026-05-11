using System.Text.Json;
using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Converters;

internal sealed class AudioStartEventConverter : EventConverterBase<AudioStartEvent>
{
    public override AudioStartEvent Convert(RawWyomingEvent @event)
    {
        return new AudioStartEvent
        {
            Data = @event.Data!.Value.Deserialize(WyomingSerializerJsonContext.Default.AudioStartEventData)!
        };
    }

    public override RawWyomingEvent Convert(AudioStartEvent @event)
    {
        return new RawWyomingEvent
        {
            Type = Constants.EventTypes.AudioStart,
            Data = JsonSerializer.SerializeToElement(@event.Data, WyomingSerializerJsonContext.Default.AudioStartEventData)
        };
    }
}
