using System.Text.Json.Serialization;
using WyomingProtocol.Models;
using WyomingProtocol.Models.Events.EventData;

namespace WyomingProtocol;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(RawWyomingEventHeader))]
[JsonSerializable(typeof(AudioChunkEventData))]
[JsonSerializable(typeof(AudioStartEventData))]
[JsonSerializable(typeof(InfoEventData))]
[JsonSerializable(typeof(TranscribeEventData))]
[JsonSerializable(typeof(TranscriptEventData))]
[JsonSerializable(typeof(SynthesizeEventData))]
[JsonSerializable(typeof(SynthesizeStartEventData))]
[JsonSerializable(typeof(SynthesizeChunkEventData))]
internal partial class WyomingSerializerJsonContext : JsonSerializerContext
{
}
