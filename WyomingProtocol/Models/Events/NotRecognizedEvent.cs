using System.Text.Json;

namespace WyomingProtocol.Models.Events;

public sealed class NotRecognizedEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.NotRecognized;

    public NotRecognizedEventData Data { get; init; } = new();
}

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
