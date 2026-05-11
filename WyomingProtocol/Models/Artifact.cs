namespace WyomingProtocol.Models;

/// <summary>
/// Information about a service, model, etc..
/// </summary>
public abstract class Artifact
{
    /// <summary>
    /// Name/id of artifact.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Who made the artifact and where it's from.
    /// </summary>
    public required Attribution Attribution { get; init; }

    /// <summary>
    /// true if the artifact is currently installed.
    /// </summary>
    public required bool Installed { get; init; }

    /// <summary>
    /// Human-readable description of the artifact.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Version of the artifact.
    /// </summary>
    public string? Version { get; init; }
}