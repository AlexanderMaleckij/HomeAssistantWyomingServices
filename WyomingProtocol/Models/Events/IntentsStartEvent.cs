using System.Text.Json;

namespace WyomingProtocol.Models.Events;

public sealed class IntentsStartEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.IntentsStart;

    public required IntentsStartEventData Data { get; init; }
}

public sealed class IntentsStartEventData
{
    /// <summary>
    /// Context from previous interactions.
    /// </summary>
    public Dictionary<string, JsonElement>? Context { get; init; }
}
