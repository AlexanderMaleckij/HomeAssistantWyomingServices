using System.Text.Json;

namespace WyomingProtocol.Models.Events.EventData;

public sealed class SynthesizeStartEventData
{
    public SynthesizeEventVoice? Voice { get; init; }

    public Dictionary<string, JsonElement>? Context { get; init; }
}
