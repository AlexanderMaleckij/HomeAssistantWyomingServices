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
[JsonSerializable(typeof(RecognizeEventData))]
[JsonSerializable(typeof(IntentEventData))]
[JsonSerializable(typeof(NotRecognizedEventData))]
[JsonSerializable(typeof(IntentsStartEventData))]
[JsonSerializable(typeof(HandledEventData))]
[JsonSerializable(typeof(NotHandledEventData))]
[JsonSerializable(typeof(HandledStartEventData))]
[JsonSerializable(typeof(HandledChunkEventData))]
internal partial class WyomingSerializerJsonContext : JsonSerializerContext
{
}
