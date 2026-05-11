namespace WyomingProtocol.Models;

/// <summary>
/// Microphone information.
/// </summary>
public sealed class MicProgram : Artifact
{
    /// <summary>
    /// Input audio format.
    /// </summary>
    public required AudioFormat MicFormat { get; init; }
}
