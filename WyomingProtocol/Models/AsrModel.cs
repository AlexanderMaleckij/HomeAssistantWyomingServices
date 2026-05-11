namespace WyomingProtocol.Models;

/// <summary>
/// Speech-to-text model.
/// </summary>
public sealed class AsrModel : Artifact
{
    /// <summary>
    /// List of supported model languages.
    /// </summary>
    public required string[] Languages { get; init; }
}
