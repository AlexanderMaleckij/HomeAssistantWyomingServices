namespace WyomingProtocol.Exceptions;

public class WyomingUnexpectedEndOfStreamException : WyomingSerializerException
{
    public WyomingUnexpectedEndOfStreamException(string message) : base(message) { }

    public WyomingUnexpectedEndOfStreamException(string message, Exception inner) : base(message, inner) { }
}
