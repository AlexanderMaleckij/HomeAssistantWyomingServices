using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WyomingPiperTtsServer;

[JsonSerializable(typeof(Dictionary<string, JsonElement>))]
[JsonSerializable(typeof(Dictionary<string, List<int>>))]
[JsonSerializable(typeof(Dictionary<string, int>))]
internal partial class PiperJsonSerializerContext : JsonSerializerContext
{
}

/// <summary>
/// Wraps a Piper ONNX text-to-speech model, converting text to PCM audio samples.
/// </summary>
internal sealed class Piper : IPiper, IDisposable
{
    // ── Phoneme boundary tokens ──────────────────────────────────────────────
    private const string BOS = "^";
    private const string EOS = "$";
    private const string PAD = "_";

    // ── Inference defaults (read once from config) ───────────────────────────
    private readonly float _defaultLengthScale;
    private readonly float _defaultNoiseScale;
    private readonly float _defaultNoiseW;

    // ── Model assets ─────────────────────────────────────────────────────────
    private readonly int _sampleRate;
    private readonly string _language;
    private readonly Dictionary<string, List<int>> _phonemeIdMap;
    private readonly Dictionary<string, int>? _speakerIdMap;
    private readonly bool _hasSpeakerInput;

    private readonly InferenceSession _session;
    private readonly Phonemizer _phonemizer;

    private bool _disposed;

    /// <inheritdoc/>
    public int SampleRate => _sampleRate;

    /// <inheritdoc/>
    public string Language => _language;

    /// <inheritdoc/>
    public IReadOnlyDictionary<string, int>? Speakers => _speakerIdMap;

    // ─────────────────────────────────────────────────────────────────────────

    /// <param name="modelPath">Path to the <c>.onnx</c> model file.</param>
    /// <param name="configPath">Path to the companion <c>.json</c> config file.</param>
    /// <param name="language">
    ///     eSpeak-NG language code used for phonemization (default: <c>"ru"</c>).
    /// </param>
    /// <param name="espeakDataDirectory">eSpeak-NG data directory.</param>
    /// <param name="espeakDllPath">Path to <c>libespeak-ng.dll</c>.</param>
    public Piper(string modelPath, string configPath, string espeakDataDirectory, string espeakDllPath)
    {
        var config = JsonSerializer.Deserialize(File.ReadAllText(configPath), PiperJsonSerializerContext.Default.DictionaryStringJsonElement)
            ?? throw new InvalidDataException("Config file could not be deserialized.");

        _sampleRate = config["audio"].GetProperty("sample_rate").GetInt32();

        _phonemeIdMap = JsonSerializer.Deserialize(config["phoneme_id_map"].GetRawText(), PiperJsonSerializerContext.Default.DictionaryStringListInt32)
            ?? throw new InvalidDataException("phoneme_id_map is missing or malformed.");

        if (config.TryGetValue("speaker_id_map", out var speakerRaw))
            _speakerIdMap = JsonSerializer.Deserialize(speakerRaw.GetRawText(), PiperJsonSerializerContext.Default.DictionaryStringInt32);

        _language = config["espeak"].GetProperty("voice").GetString()
            ?? throw new InvalidDataException("espeak.voice is missing from config.");

        var inferenceCfg = config["inference"];
        _defaultLengthScale = (float)inferenceCfg.GetProperty("length_scale").GetDouble();
        _defaultNoiseScale = (float)inferenceCfg.GetProperty("noise_scale").GetDouble();
        _defaultNoiseW = (float)inferenceCfg.GetProperty("noise_w").GetDouble();

        var sessionOptions = new SessionOptions();
        sessionOptions.EnableMemoryPattern = false;
        sessionOptions.LogSeverityLevel = OrtLoggingLevel.ORT_LOGGING_LEVEL_ERROR;
        sessionOptions.GraphOptimizationLevel = GraphOptimizationLevel.ORT_ENABLE_ALL;
        sessionOptions.AppendExecutionProvider_DML(0);

        _session = new InferenceSession(modelPath, sessionOptions);
        _hasSpeakerInput = _session.InputMetadata.ContainsKey("sid");

        _phonemizer = new Phonemizer(
            defaultVoice: _language,
            espeakNgDataDirectory: espeakDataDirectory,
            espeakNgDllPath: espeakDllPath);
    }

    // ── Public API ────────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public float[] Synthesize(
        string text,
        string? speakerName = null,
        float? lengthScale = null,
        float? noiseScale = null,
        float? noiseW = null)
    {
        var inputs = BuildInputs(
            text, speakerName,
            lengthScale ?? _defaultLengthScale,
            noiseScale ?? _defaultNoiseScale,
            noiseW ?? _defaultNoiseW);

        using var results = _session.Run(inputs);
        return results.First().AsEnumerable<float>().ToArray();
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<float[]> SynthesizeStreamingAsync(
        string text,
        string? speakerName = null,
        float? lengthScale = null,
        float? noiseScale = null,
        float? noiseW = null,
        [System.Runtime.CompilerServices.EnumeratorCancellation]
        CancellationToken cancellationToken = default)
    {
        foreach (var sentence in SplitSentences(text))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(sentence))
                continue;

            // Offload each ONNX inference call to the thread-pool so the
            // caller's synchronization context is not blocked.
            var chunk = await Task.Run(() =>
            {
                var inputs = BuildInputs(
                    sentence, speakerName,
                    lengthScale ?? _defaultLengthScale,
                    noiseScale ?? _defaultNoiseScale,
                    noiseW ?? _defaultNoiseW);

                using var results = _session.Run(inputs);
                return results.First().AsEnumerable<float>().ToArray();
            }, cancellationToken);

            yield return chunk;
        }
    }

    // ── Private helpers ───────────────────────────────────────────────────────

    private List<NamedOnnxValue> BuildInputs(
        string text,
        string? speakerName,
        float lengthScale,
        float noiseScale,
        float noiseW)
    {
        var ids = PhonemeToIds(Phonemize(text));

        var inputTensor = new DenseTensor<long>(new[] { 1, ids.Count });
        for (int i = 0; i < ids.Count; i++)
            inputTensor[0, i] = ids[i];

        var lengthTensor = new DenseTensor<long>(new[] { 1 }) { [0] = ids.Count };

        var scalesTensor = new DenseTensor<float>(new[] { 3 });
        scalesTensor[0] = noiseScale;
        scalesTensor[1] = lengthScale;
        scalesTensor[2] = noiseW;

        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("input",         inputTensor),
            NamedOnnxValue.CreateFromTensor("input_lengths", lengthTensor),
            NamedOnnxValue.CreateFromTensor("scales",        scalesTensor),
        };

        if (_hasSpeakerInput)
        {
            var sidTensor = new DenseTensor<long>(new[] { 1 }) { [0] = ResolveSpeakerId(speakerName) };
            inputs.Add(NamedOnnxValue.CreateFromTensor("sid", sidTensor));
        }

        return inputs;
    }

    private int ResolveSpeakerId(string? speakerName) =>
        speakerName is not null && _speakerIdMap?.TryGetValue(speakerName, out int id) == true
            ? id
            : 0;

    private List<int> PhonemeToIds(IEnumerable<string> phonemes)
    {
        var ids = new List<int>();
        foreach (var p in phonemes)
        {
            if (_phonemeIdMap.TryGetValue(p, out var idList))
            {
                ids.AddRange(idList);
                ids.AddRange(_phonemeIdMap[PAD]);
            }
        }
        ids.AddRange(_phonemeIdMap[EOS]);
        return ids;
    }

    private IEnumerable<string> Phonemize(string text)
    {
        yield return BOS;
        foreach (var sentence in SplitSentences(text))
        {
            var phonemeStr = _phonemizer.Phonemize(
                sentence,
                voice: _language,
                noStress: false,
                keepLanguageFlags: true,
                punctuationSeparator: ",",
                keepClauseBreakers: true,
                phonemeSeparator: null);

            foreach (char c in phonemeStr)
                yield return c.ToString();
        }
    }

    /// <summary>Splits text on sentence boundaries.</summary>
    private static IEnumerable<string> SplitSentences(string text) =>
        text.Split(new[] { ". ", "! ", "? " }, StringSplitOptions.RemoveEmptyEntries);

    // ── IDisposable ───────────────────────────────────────────────────────────

    public void Dispose()
    {
        if (_disposed) return;
        _session.Dispose();
        _phonemizer.Dispose();
        _disposed = true;
    }
}
