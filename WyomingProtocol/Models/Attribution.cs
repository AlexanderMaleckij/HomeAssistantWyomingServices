namespace WyomingProtocol.Models;

/// <summary>
/// Attribution for an artifact.
/// </summary>
public sealed class Attribution
{
    /// <summary>
    /// Who made it.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Where it's from.
    /// </summary>
    public required string Url { get; init; }
}
