using System.Text.Json;

namespace WyomingProtocol.Models.Events.EventData;

public sealed class IntentsStartEventData
{
    /// <summary>
    /// Context from previous interactions.
    /// </summary>
    public Dictionary<string, JsonElement>? Context { get; init; }
}
