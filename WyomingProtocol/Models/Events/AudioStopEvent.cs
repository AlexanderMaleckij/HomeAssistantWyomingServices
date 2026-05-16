namespace WyomingProtocol.Models.Events;

public sealed class AudioStopEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.AudioStop;

    /// <summary>
    /// Milliseconds
    /// </summary>
    public int? Timestamp { get; init; }
}
