namespace WyomingProtocol.Exceptions;

public class WyomingException : Exception
{
	public WyomingException() { }

	public WyomingException(string message) : base(message) { }

	public WyomingException(string message, Exception inner) : base(message, inner) { }
}
