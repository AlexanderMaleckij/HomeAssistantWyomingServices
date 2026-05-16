using WyomingProtocol.Converters.Shared;
using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Converters;

internal sealed class AudioStopEventConverter : EventConverterBase<AudioStopEvent>
{
    public override AudioStopEvent Convert(RawWyomingEvent @event)
    {
        return new AudioStopEvent();
    }

    public override RawWyomingEvent Convert(AudioStopEvent @event)
    {
        return new RawWyomingEvent
        {
            Type = @event.Type
        };
    }
}
