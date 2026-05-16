using System.Text.Json;
using WyomingProtocol.Converters.Shared;
using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Converters;

internal sealed class InfoEventConverter : EventConverterBase<InfoEvent>
{
    public override InfoEvent Convert(RawWyomingEvent @event)
    {
        return new InfoEvent
        {
            Data = @event.Data!.Value.Deserialize(WyomingSerializerJsonContext.Default.InfoEventData)!
        };
    }

    public override RawWyomingEvent Convert(InfoEvent @event)
    {
        return new RawWyomingEvent
        {
            Type = @event.Type,
            Data = JsonSerializer.SerializeToElement(@event.Data, WyomingSerializerJsonContext.Default.InfoEventData)
        };
    }
}
