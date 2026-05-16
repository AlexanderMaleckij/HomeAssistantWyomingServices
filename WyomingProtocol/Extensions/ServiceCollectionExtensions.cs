using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using WyomingProtocol.Converters;
using WyomingProtocol.Converters.Shared;
using WyomingProtocol.Server;
using WyomingProtocol.Services;

using static WyomingProtocol.Constants.EventTypes;

namespace WyomingProtocol.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddWyomingProtocol()
        {
            // Serialization services.
            services.AddSingleton<IRawWyomingSerializer, RawWyomingSerializer>();
            services.AddSingleton<IWyomingSerializer, WyomingSerializer>();
            services.AddSingleton<IEventConverterProvider, EventConverterProvider>();

            // Default converters.
            services.AddWyomingEventConverter<InfoEventConverter>(Info);
            services.AddWyomingEventConverter<AudioChunkEventConverter>(AudioChunk);
            services.AddWyomingEventConverter<AudioStartEventConverter>(AudioStart);
            services.AddWyomingEventConverter<AudioStopEventConverter>(AudioStop);
            services.AddWyomingEventConverter<DescribeEventConverter>(Describe);
            services.AddWyomingEventConverter<TranscribeEventConverter>(Transcribe);
            services.AddWyomingEventConverter<TranscriptEventConverter>(Transcript);
            services.AddWyomingEventConverter<SynthesizeEventConverter>(Synthesize);
            services.AddWyomingEventConverter<SynthesizeStartEventConverter>(SynthesizeStart);
            services.AddWyomingEventConverter<SynthesizeChunkEventConverter>(SynthesizeChunk);
            services.AddWyomingEventConverter<SynthesizeStopEventConverter>(SynthesizeStop);
            services.AddWyomingEventConverter<SynthesizeStoppedEventConverter>(SynthesizeStopped);
            services.AddWyomingEventConverter<RecognizeEventConverter>(Recognize);
            services.AddWyomingEventConverter<IntentEventConverter>(Intent);
            services.AddWyomingEventConverter<NotRecognizedEventConverter>(NotRecognized);
            services.AddWyomingEventConverter<IntentsStartEventConverter>(IntentsStart);
            services.AddWyomingEventConverter<IntentsStopEventConverter>(IntentsStop);
            services.AddWyomingEventConverter<HandledEventConverter>(Handled);
            services.AddWyomingEventConverter<NotHandledEventConverter>(NotHandled);
            services.AddWyomingEventConverter<HandledStartEventConverter>(HandledStart);
            services.AddWyomingEventConverter<HandledChunkEventConverter>(HandledChunk);
            services.AddWyomingEventConverter<HandledStopEventConverter>(HandledStop);

            // Server.
            services.AddSingleton<IWyomingServerFactory, WyomingServerFactory>();

            return services;
        }

        public IServiceCollection AddWyomingEventConverter<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TCoverter>(
            string eventType)
            where TCoverter : class, IEventConverter, new()
        {
            return services.AddKeyedSingleton<IEventConverter, TCoverter>(eventType);
        }

        public IServiceCollection AddWyomingEventHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler>(string key)
            where THandler : class, IWyomingEventHandler
        {
            return services.AddKeyedScoped<IWyomingEventHandler, THandler>(key);
        }
    }
}
