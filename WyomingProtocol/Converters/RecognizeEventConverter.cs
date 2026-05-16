using System.Text.Json;
using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Converters;

internal sealed class RecognizeEventConverter : EventConverterBase<RecognizeEvent>
{
    public override RecognizeEvent Convert(RawWyomingEvent @event)
    {
        return new RecognizeEvent
        {
            Data = @event.Data!.Value.Deserialize(WyomingSerializerJsonContext.Default.RecognizeEventData)!
        };
    }

    public override RawWyomingEvent Convert(RecognizeEvent @event)
    {
        return new RawWyomingEvent
        {
            Type = Constants.EventTypes.Recognize,
            Data = JsonSerializer.SerializeToElement(@event.Data, WyomingSerializerJsonContext.Default.RecognizeEventData)
        };
    }
}
