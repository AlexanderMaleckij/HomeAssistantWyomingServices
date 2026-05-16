namespace WyomingProtocol.Models.Events;

public sealed class AudioStartEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.AudioStart;

    public required AudioStartEventData Data { get; init; }
}

public sealed class AudioStartEventData : AudioFormat
{
    /// <summary>
    /// Milliseconds
    /// </summary>
    public int? Timestamp { get; init; }
}
