using System.Text.Json;

namespace WyomingProtocol.Models;

/// <summary>
/// The header is a single-line JSON object terminated with a newline (\n).
/// </summary>
internal sealed class RawWyomingEventHeader
{
    /// <summary>
    /// Identifies the type of event (e.g., "audio-chunk", "transcript").
    /// </summary>
    public required string Type { get; init; }

    public string? Version { get; init; }

    /// <summary>
    /// Length in bytes of the additional JSON data section.
    /// </summary>
    public int? DataLength { get; init; }

    /// <summary>
    /// Length in bytes of the binary payload.
    /// </summary>
    public int? PayloadLength { get; init; }

    /// <summary>
    /// Small inline data that can be included directly in the header.
    /// </summary>
    public JsonElement? Data { get; init; }
}
