using System.Text.Json;

namespace WyomingProtocol.Models.Events;

public sealed class NotRecognizedEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.NotRecognized;

    public required NotRecognizedEventData Data { get; init; }
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
