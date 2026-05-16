using System.Text.Json;

namespace WyomingProtocol.Models.Events.EventData;

public sealed class RecognizeEventData
{
    /// <summary>
    /// Text to recognize.
    /// </summary>
    public required string Text { get; init; }

    /// <summary>
    /// Context from previous interactions.
    /// </summary>
    public Dictionary<string, JsonElement>? Context { get; init; }
}
