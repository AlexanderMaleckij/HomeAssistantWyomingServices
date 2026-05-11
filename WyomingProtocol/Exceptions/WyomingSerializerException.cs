namespace WyomingProtocol.Exceptions;

/// <summary>
/// Base exception for Wyoming protocol serialization/deserialization errors.
/// </summary>
public class WyomingSerializerException : WyomingException
{
    public WyomingSerializerException(string message) : base(message) { }

    public WyomingSerializerException(string message, Exception inner) : base(message, inner) { }
}
