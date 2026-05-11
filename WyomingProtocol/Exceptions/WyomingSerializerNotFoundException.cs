using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Exceptions;

public class WyomingSerializerNotFoundException : WyomingSerializerException
{
    public RawWyomingEvent? RawEvent { get; }

    public WyomingSerializerNotFoundException(string message)
        : base(message)
    {
    }

    public WyomingSerializerNotFoundException(string message, RawWyomingEvent rawEvent)
        : base(message)
    {
        RawEvent = rawEvent;
    }
}
