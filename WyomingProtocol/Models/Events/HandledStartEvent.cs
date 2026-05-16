using System.Text.Json;

namespace WyomingProtocol.Models.Events;

public sealed class HandledStartEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.HandledStart;

    public required HandledStartEventData Data { get; init; }
}

public sealed class HandledStartEventData
{
    /// <summary>
    /// Context from previous interactions.
    /// </summary>
    public Dictionary<string, JsonElement>? Context { get; init; }
}
