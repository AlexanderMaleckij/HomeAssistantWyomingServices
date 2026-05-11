namespace WyomingProtocol.Exceptions;

public class WyomingZeroLengthHeaderException : WyomingSerializerException
{
    public WyomingZeroLengthHeaderException(string message) : base(message) { }


    public WyomingZeroLengthHeaderException(string message, Exception inner) : base(message, inner) { }
}
