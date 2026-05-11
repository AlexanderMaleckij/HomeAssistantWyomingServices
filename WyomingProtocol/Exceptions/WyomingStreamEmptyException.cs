namespace WyomingProtocol.Exceptions;

public class WyomingStreamEmptyException : WyomingSerializerException
{
    public WyomingStreamEmptyException(string message) : base(message) { }

    public WyomingStreamEmptyException(string message, Exception inner) : base(message, inner) { }
}
