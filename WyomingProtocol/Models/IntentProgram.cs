namespace WyomingProtocol.Models;

/// <summary>
/// Intent recognition service.
/// </summary>
public sealed class IntentProgram : Artifact
{
    /// <summary>
    /// List of available models.
    /// </summary>
    public required IntentModel[] Models { get; init; }
}
