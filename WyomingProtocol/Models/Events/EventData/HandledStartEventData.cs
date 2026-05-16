using System.Text.Json;

namespace WyomingProtocol.Models.Events.EventData;

public sealed class HandledStartEventData
{
    /// <summary>
    /// Context from previous interactions.
    /// </summary>
    public Dictionary<string, JsonElement>? Context { get; init; }
}
