using System.Text.Json;

namespace WyomingProtocol.Models.Events;

public sealed class HandledEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.Handled;

    public required HandledEventData Data { get; init; }
}

public sealed class HandledEventData
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
