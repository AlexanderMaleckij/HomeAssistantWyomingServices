namespace WyomingPiperTtsServer;

internal interface IPiper : IDisposable
{
    /// <summary>
    /// The sample rate (Hz) of audio produced by this model.
    /// </summary>
    int SampleRate { get; }

    /// <summary>
    /// The eSpeak-NG voice code read from <c>espeak.voice</c> in the model config.
    /// </summary>
    string Language { get; }

    /// <summary>
    /// Speaker name → ID mapping for multi-speaker models; <c>null</c> for single-speaker models.
    /// </summary>
    IReadOnlyDictionary<string, int>? Speakers { get; }

    /// <summary>
    /// Synthesizes <paramref name="text"/> and returns the full PCM audio buffer.
    /// </summary>
    /// <param name="text">Input text (plain text, not pre-phonemized).</param>
    /// <param name="speakerName">
    ///     Speaker name for multi-speaker models.
    ///     Pass <c>null</c> (default) for single-speaker models or speaker 0.
    /// </param>
    /// <param name="lengthScale">
    ///     Controls speaking rate. Values &gt; 1 slow down; &lt; 1 speed up.
    ///     Defaults to the value in the model config.
    /// </param>
    /// <param name="noiseScale">
    ///     Controls variation in pitch/timing. Defaults to the model config value.
    /// </param>
    /// <param name="noiseW">
    ///     Controls phoneme duration noise. Defaults to the model config value.
    /// </param>
    /// <returns>Raw 32-bit float PCM samples at <see cref="SampleRate"/> Hz.</returns>
    float[] Synthesize(
        string text,
        string? speakerName = null,
        float? lengthScale = null,
        float? noiseScale = null,
        float? noiseW = null);

    /// <summary>
    /// Synthesizes <paramref name="text"/> sentence-by-sentence, yielding each
    /// PCM chunk as soon as it is ready. Suitable for low-latency streaming playback.
    /// </summary>
    /// <inheritdoc cref="Synthesize" select="param"/>
    /// <returns>
    ///     An async sequence of PCM chunks, one per sentence in the source text.
    /// </returns>
    IAsyncEnumerable<float[]> SynthesizeStreamingAsync(
        string text,
        string? speakerName = null,
        float? lengthScale = null,
        float? noiseScale = null,
        float? noiseW = null,
        CancellationToken cancellationToken = default);
}
