namespace WyomingProtocol.Models;

/// <summary>
/// Sound output information.
/// </summary>
public sealed class SndProgram : Artifact
{
    /// <summary>
    /// Output audio format.
    /// </summary>
    public required AudioFormat SndFormat { get; init; }
}
