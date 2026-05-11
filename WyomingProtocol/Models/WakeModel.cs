namespace WyomingProtocol.Models;

/// <summary>
/// Wake word detection model.
/// </summary>
public sealed class WakeModel : Artifact
{
    /// <summary>
    /// List of languages supported by the model.
    /// </summary>
    public required string[] Languages { get; init; }

    /// <summary>
    /// Wake up phrase used by the model.
    /// </summary>
    public string? Phrase { get; init; }
}
