using System.Text.Json;

namespace WyomingProtocol.Models.Events;

public sealed class SynthesizeStartEvent : IWyomingEvent
{
    public string Type => Constants.EventTypes.SynthesizeStart;

    public SynthesizeStartEventData? Data { get; init; }
}

public sealed class SynthesizeStartEventData
{
    public SynthesizeEventVoice? Voice { get; init; }

    public Dictionary<string, JsonElement>? Context { get; init; }
}
