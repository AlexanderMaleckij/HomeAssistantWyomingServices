using System.Text.Json;

namespace WyomingProtocol.Models.Events;

public sealed class RawWyomingEvent : IWyomingEvent
{
    public required string Type { get; init; }

    public string? Version { get; init; }

    public JsonElement? Data { get; init; }

    public byte[]? Payload { get; init; }

    internal RawWyomingEvent()
    {
    }
}
