namespace WyomingProtocol.Models;

/// <summary>
/// Intent handling model.
/// </summary>
public sealed class HandleModel : Artifact
{
    /// <summary>
    /// List of supported languages in the model.
    /// </summary>
    public required string[] Languages { get; init; }
}
