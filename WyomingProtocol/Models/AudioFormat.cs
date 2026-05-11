namespace WyomingProtocol.Models;

/// <summary>
/// Base class for events with audio format information.
/// </summary>
public abstract class AudioFormat
{
    /// <summary>
    /// Hertz
    /// </summary>
    public required int Rate { get; init; }

    /// <summary>
    /// Bytes
    /// </summary>
    public required int Width { get; init; }

    /// <summary>
    /// Mono = 1
    /// </summary>
    public required int Channels { get; init; }
}
