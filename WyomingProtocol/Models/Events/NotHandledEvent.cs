using System.Text.Json;

namespace WyomingProtocol.Models.Events;

public sealed class NotHandledEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.NotHandled;

    public required NotHandledEventData Data { get; init; }
}

public sealed class NotHandledEventData
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
