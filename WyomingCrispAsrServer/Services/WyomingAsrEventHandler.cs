using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WyomingCrispAsrServer.Models.Options.WyomingAsrServer;
using WyomingProtocol.Models;
using WyomingProtocol.Models.Events;
using WyomingProtocol.Models.Events.EventData;
using WyomingProtocol.Server;

namespace WyomingCrispAsrServer.Services
{
    internal sealed class WyomingAsrEventHandler : IWyomingEventHandler, IDisposable
    {
        private string? _requestLanguage;
        private CrispAsrStreamingSession? _session;
        private readonly IOptions<WyomingAsrServerOptions> _options;
        private readonly ILogger<WyomingAsrEventHandler> _logger;

        public WyomingAsrEventHandler(
            IOptions<WyomingAsrServerOptions> options,
            ILogger<WyomingAsrEventHandler> logger)
        {
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(logger);

            _options = options;
            _logger = logger;
        }

        public void Dispose()
        {
            _session?.Dispose();
        }

        public async Task HandleAsync(IWyomingEvent wyomingEvent, IWyomingRequestContext context, CancellationToken cancellationToken)
        {
            switch (wyomingEvent)
            {
                case DescribeEvent:
                    {
                        await context.RespondAsync(CreateInfoEvent(_options.Value), cancellationToken);
                        context.CloseConnection();
                        return;
                    }
                case TranscribeEvent transcribeEvent:
                    {
                        _logger.LogInformation(
                            "ASR request: {model} model, {language} language",
                            transcribeEvent.Data.Name,
                            transcribeEvent.Data.Language);

                        _session?.Dispose();

                        var configuration = _options.Value;
                        WyomingAsrServerModel modelConfiguration;

                        if (transcribeEvent.Data.Name is null)
                        {
                            modelConfiguration = configuration.Models.First();

                            if (configuration.Models.Length > 1)
                            {
                                throw new Exception("Client did not provide an ASR model name when more than one model is available.");
                            }
                        }
                        else
                        {
                            modelConfiguration = configuration.Models.First(m => m.Name == transcribeEvent.Data.Name);
                        }

                        var crispAsrExecutable = new FileInfo(configuration.CrispAsrPath);

                        _requestLanguage = GetRequestLanguage(transcribeEvent, modelConfiguration);
                        _session = new CrispAsrStreamingSession(crispAsrExecutable, modelConfiguration.ModelFile, _requestLanguage);

                        return;
                    }
                case AudioStartEvent audioStartEvent:
                    {
                        _logger.LogInformation(
                            "Audio stream started: {rate}Hz, {width} bytes, {channels} channels",
                            audioStartEvent.Data.Rate,
                            audioStartEvent.Data.Width,
                            audioStartEvent.Data.Channels);

                        return;
                    }
                case AudioChunkEvent audioChunkEvent:
                    {
                        await _session!.AddPcmDataAsync(audioChunkEvent.Payload, cancellationToken);

                        _logger.LogDebug(
                            "Audio chunk received: {size} bytes. {timestamp} timestamp {rate}Hz, {width} bytes, {channels} channels",
                            audioChunkEvent.Payload.Length,
                            audioChunkEvent.Data.Timestamp,
                            audioChunkEvent.Data.Rate,
                            audioChunkEvent.Data.Width,
                            audioChunkEvent.Data.Channels);

                        return;
                    }
                case AudioStopEvent:
                    {
                        _logger.LogInformation("Audio stream stopped.");

                        var transcription = await _session!.GetTranscriptionAsync(cancellationToken);

                        _logger.LogDebug("Transcription: {transcription}", transcription);
                        _logger.LogInformation("Sending transcription.");

                        await context.RespondAsync(
                            new TranscriptEvent
                            {
                                Data = new TranscriptEventData
                                {
                                    Text = transcription,
                                    Language = _requestLanguage
                                }
                            }, cancellationToken);

                        context.CloseConnection();
                        return;
                    }
                default:
                    _logger.LogDebug("Unknown event type: {type}", wyomingEvent.Type);
                    return;
            }
        }

        private string? GetRequestLanguage(TranscribeEvent transcribeEvent, WyomingAsrServerModel modelConfiguration)
        {
            if (transcribeEvent.Data.Language is not null)
            {
                return transcribeEvent.Data.Language;
            }

            if (!modelConfiguration.SupportsLanguageAutoDetect)
            {
                return _options.Value.FallbackLanguage;
            }

            return null;
        }

        private static InfoEvent CreateInfoEvent(WyomingAsrServerOptions options)
        {
            return new InfoEvent
            {
                Data = new InfoEventData
                {
                    Asr = [
                        new AsrProgram
                        {
                            Name = "CrispASR STT",
                            Attribution = new Attribution
                            {
                                Name = "The CrispASR Project",
                                Url = "https://github.com/CrispStrobe/CrispASR",
                            },
                            Installed = true,
                            Description = "CrispASR STT Integration",
                            Version = "1.0.0",
                            Models = MapModels(options.Models)
                        }
                    ]
                }
            };

            static AsrModel[] MapModels(WyomingAsrServerModel[] models) => [
                .. models.Select(static model => new AsrModel
                {
                    Name = model.Name,
                    Attribution = new Attribution
                    {
                        Name = model.Attribution.Name,
                        Url = model.Attribution.Url
                    },
                    Installed = true,
                    Description = model.Description,
                    Version = model.Version,
                    Languages = model.Languages,
                })
            ];
        }
    }
}
