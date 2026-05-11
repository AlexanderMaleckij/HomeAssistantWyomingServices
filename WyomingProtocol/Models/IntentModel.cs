namespace WyomingProtocol.Models;

/// <summary>
/// Intent recognition model.
/// </summary>
public sealed class IntentModel : Artifact
{
    /// <summary>
    /// List of languages supported by the model.
    /// </summary>
    public required string[] Languages { get; init; }
}
