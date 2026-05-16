using System.Text.Json;

namespace WyomingProtocol.Models.Events.EventData;

public sealed class NotRecognizedEventData
{
    /// <summary>
    /// Response for user.
    /// </summary>
    public string? Text { get; init; }

    /// <summary>
    /// Context for next interactions.
    /// </summary>
    public Dictionary<string, JsonElement>? Context { get; init; }
}
