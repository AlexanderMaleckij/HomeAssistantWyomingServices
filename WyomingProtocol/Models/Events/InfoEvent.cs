namespace WyomingProtocol.Models.Events;

public sealed class InfoEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.Info;

    public required InfoEventData Data { get; init; }
}

/// <summary>
/// Response to describe message with information about available services, models, etc.
/// </summary>
public sealed class InfoEventData
{
    /// <summary>
    /// Speech-to-text services.
    /// </summary>
    public AsrProgram[]? Asr { get; init; }

    /// <summary>
    /// Text-to-speech services.
    /// </summary>
    public TtsProgram[]? Tts { get; init; }

    /// <summary>
    /// Intent handling services.
    /// </summary>
    public HandleProgram[]? Handle { get; init; }

    /// <summary>
    /// Intent recognition services.
    /// </summary>
    public IntentProgram[]? Intent { get; init; }

    /// <summary>
    /// Wake word detection services.
    /// </summary>
    public WakeProgram[]? Wake { get; init; }

    /// <summary>
    /// Audio input services.
    /// </summary>
    public MicProgram[]? Mic { get; init; }

    /// <summary>
    /// Audio output services.
    /// </summary>
    public SndProgram[]? Snd { get; init; }

    /// <summary>
    /// Satellite information.
    /// </summary>
    public Satellite? Satellite { get; init; }
}
