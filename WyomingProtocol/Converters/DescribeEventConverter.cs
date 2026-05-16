using WyomingProtocol.Converters.Shared;
using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Converters;

internal sealed class DescribeEventConverter : EventConverterBase<DescribeEvent>
{
    public override DescribeEvent Convert(RawWyomingEvent @event)
    {
        return new DescribeEvent();
    }

    public override RawWyomingEvent Convert(DescribeEvent @event)
    {
        return new RawWyomingEvent
        {
            Type = @event.Type
        };
    }
}
