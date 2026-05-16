using System.Text.Json;
using WyomingProtocol.Converters.Shared;
using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Converters
{
    internal sealed class SynthesizeEventConverter : EventConverterBase<SynthesizeEvent>
    {
        public override SynthesizeEvent Convert(RawWyomingEvent @event)
        {
            return new SynthesizeEvent
            {
                Data = @event.Data!.Value.Deserialize(WyomingSerializerJsonContext.Default.SynthesizeEventData)!
            };
        }

        public override RawWyomingEvent Convert(SynthesizeEvent @event)
        {
            return new RawWyomingEvent
            {
                Type = @event.Type,
                Data = JsonSerializer.SerializeToElement(@event.Data, WyomingSerializerJsonContext.Default.SynthesizeEventData)
            };
        }
    }
}
