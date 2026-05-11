namespace WyomingProtocol.Models.Events.EventData;

public sealed class AudioStartEventData : AudioFormat
{
    /// <summary>
    /// Milliseconds
    /// </summary>
    public int? Timestamp { get; init; }
}
