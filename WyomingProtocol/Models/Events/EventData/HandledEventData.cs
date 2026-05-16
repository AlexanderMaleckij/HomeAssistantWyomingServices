using System.Text.Json;

namespace WyomingProtocol.Models.Events.EventData;

internal class HandledEventData
{
    /// <summary>
    /// Response for user.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Context for next interactions.
    /// </summary>
    public Dictionary<string, JsonElement>? Context { get; init; }
}
