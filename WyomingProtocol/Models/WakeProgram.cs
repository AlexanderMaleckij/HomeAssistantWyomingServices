namespace WyomingProtocol.Models;

/// <summary>
/// Wake word detection service.
/// </summary>
public sealed class WakeProgram : Artifact
{
    /// <summary>
    /// List of available models.
    /// </summary>
    public required WakeModel[] Models { get; init; }
}
