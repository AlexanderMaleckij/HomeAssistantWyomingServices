using System.Text.Json;

namespace WyomingProtocol.Models.Events;

public sealed class IntentEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.Intent;

    public required IntentEventData Data { get; init; }
}

public sealed class IntentEventData
{
    /// <summary>
    /// Name of intent.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// List of entities.
    /// </summary>
    public Intent[]? Entities { get; init; }

    /// <summary>
    /// Response for user.
    /// </summary>
    public string? Text { get; init; }

    /// <summary>
    /// Context for next interactions.
    /// </summary>
    public Dictionary<string, JsonElement>? Context { get; init; }
}

public sealed class Intent
{
    /// <summary>
    /// Name of entity.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Value of entity.
    /// </summary>
    public JsonElement? Value { get; init; }
}
