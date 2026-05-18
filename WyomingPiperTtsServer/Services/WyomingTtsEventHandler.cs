using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Buffers.Binary;
using WyomingPiperTtsServer.Models.Options;
using WyomingProtocol.Models;
using WyomingProtocol.Models.Events;
using WyomingProtocol.Server;

namespace WyomingPiperTtsServer.Services;

internal sealed class WyomingTtsEventHandler : IWyomingEventHandler, IDisposable
{
    private IPiper? _piper;
    private readonly IPiperProvider _piperProvider;
    private readonly IOptions<PiperOptions> _options;
    private readonly ILogger<WyomingTtsEventHandler> _logger;

    public WyomingTtsEventHandler(
        IPiperProvider piperProvider,
        IOptions<PiperOptions> options,
        ILogger<WyomingTtsEventHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(piperProvider);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _piperProvider = piperProvider;
        _options = options;
        _logger = logger;
    }

    public void Dispose()
    {
        _piper?.Dispose();
    }

    public async Task HandleAsync(IWyomingEvent wyomingEvent, IWyomingRequestContext context, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received '{evntType}' event.", wyomingEvent.Type);

        switch (wyomingEvent)
        {
            case DescribeEvent:
                {
                    await context.RespondAsync(CreateInfoEvent(_options.Value), cancellationToken);
                    context.CloseConnection();
                    return;
                }
            case SynthesizeStartEvent startEvent:
                {
                    _piper = _piperProvider.GetPiper(startEvent.Data?.Voice?.Name);
                    return;
                }
            case SynthesizeChunkEvent:
                {
                    // TODO: Handle chunks.
                    return;
                }
            case SynthesizeEvent synthesizeEvent:
                {
                    _piper ??= _piperProvider.GetPiper(synthesizeEvent.Data.Voice?.Name);

                    await context.RespondAsync(new AudioStartEvent
                    {
                        Data = new AudioStartEventData
                        {
                            Rate = _piper.SampleRate,
                            Width = 2,
                            Channels = 1,
                        }
                    }, cancellationToken);

                    // Synchronous option
                    //var data = _piper.Synthesize(synthesizeEvent.Data.Text);
                    //var converted = FloatToPcm16(data);
                    //await context.RespondAsync(new AudioChunkEvent { Data = new AudioChunkEventData { Rate = _piper.SampleRate, Width = 2, Channels = 1 }, Payload = converted }, cancellationToken);

                    await foreach (var chunk in _piper.SynthesizeStreamingAsync(
                        synthesizeEvent.Data.Text,
                        speakerName: synthesizeEvent.Data.Voice?.Speaker,
                        cancellationToken: cancellationToken))
                    {
                        await context.RespondAsync(new AudioChunkEvent
                        {
                            Data = new AudioChunkEventData
                            {
                                Rate = _piper.SampleRate,
                                Width = 2,
                                Channels = 1,
                            },
                            Payload = FloatToPcm16(chunk),
                        }, cancellationToken);
                    }

                    await context.RespondAsync(new AudioStopEvent(), cancellationToken);

                    return;
                }
            case SynthesizeStopEvent:
                {
                    context.CloseConnection();

                    return;
                }
            default:
                _logger.LogDebug("Unknown event type: {type}", wyomingEvent.Type);
                return;
        }
    }

    private static byte[] FloatToPcm16(float[] samples)
    {
        var bytes = new byte[samples.Length * 2]; // 2 bytes per sample
        for (int i = 0; i < samples.Length; i++)
        {
            // Clamp, scale to Int16 range, convert
            float clamped = Math.Clamp(samples[i], -1f, 1f);
            short pcm = (short)(clamped * short.MaxValue);
            BinaryPrimitives.WriteInt16LittleEndian(bytes.AsSpan(i * 2), pcm);
        }
        return bytes;
    }

    private static InfoEvent CreateInfoEvent(PiperOptions options)
    {
        var voices = options.Models.Select(model => new TtsVoice
        {
            Name = model.Id,
            Attribution = new Attribution
            {
                Name = model.Attribution.Name,
                Url = model.Attribution.Url
            },
            Installed = true,
            Description = model.Description,
            Version = model.Version,
            Languages = model.Languages,
            Speakers = model.Speakers?.Select(speaker => new TtsModelSpeaker { Name = speaker }).ToArray(),

        }).ToArray();

        return new InfoEvent
        {
            Data = new InfoEventData
            {
                Tts = [
                    new TtsProgram
                    {
                        Name = "Piper TTS",
                        Attribution = new Attribution
                        {
                            Name = "The Piper Project",
                            Url = "https://github.com/OHF-Voice/piper1-gpl",
                        },
                        Installed = true,
                        Description = "Piper TTS Integration",
                        Version = "1.0.0",
                        Voices = voices,
                        SupportsSynthesizeStreaming = true
                    }
                ]
            }
        };
    }
}
