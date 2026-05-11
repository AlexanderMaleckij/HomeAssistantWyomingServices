using WyomingProtocol.Exceptions;
using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Services;

internal sealed class WyomingSerializer : IWyomingSerializer
{
    private readonly IRawWyomingSerializer _rawWyomingSerializer;
    private readonly IEventConverterProvider _eventConverterProvider;

    public WyomingSerializer(
        IRawWyomingSerializer rawWyomingSerializer,
        IEventConverterProvider eventSerializerProvider)
    {
        ArgumentNullException.ThrowIfNull(rawWyomingSerializer);
        ArgumentNullException.ThrowIfNull(eventSerializerProvider);

        _rawWyomingSerializer = rawWyomingSerializer;
        _eventConverterProvider = eventSerializerProvider;
    }

    public async Task<IWyomingEvent> DeserializeAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        try
        {
            var rawEvent = await _rawWyomingSerializer.DeserializeAsync(stream, cancellationToken);
            var converter = _eventConverterProvider.GetSerializer(rawEvent.Type);

            if (converter is null)
            {
                // TODO: Make this behavior configurable.
                return rawEvent;

                throw new WyomingSerializerNotFoundException(
                    $"Wyoming event deserialization failed because a converter for type '{rawEvent.Type}' was not found.", rawEvent);
            }

            return converter.Convert(rawEvent);
        }
        catch (Exception ex) when (ex is not WyomingException)
        {
            throw new WyomingSerializerException("An unexpected error occurred while deserializing a Wyoming protocol message.", ex);
        }
    }

    public async Task<Stream> SerializeAsync(IWyomingEvent @event, CancellationToken cancellationToken = default)
    {
        try
        {
            RawWyomingEvent rawEvent;

            // TODO: Make this behavior configurable.
            if (@event is RawWyomingEvent rawWyomingEvent)
            {
                rawEvent = rawWyomingEvent;
            }
            else
            {
                var converter = _eventConverterProvider.GetSerializer(@event.Type);

                if (converter is null)
                {
                    throw new WyomingSerializerNotFoundException(
                        $"Wyoming event serialization failed because a converter for type '{@event.Type}' was not found.");
                }

                rawEvent = converter.Convert(@event);
            }

            return await _rawWyomingSerializer.SerializeAsync(rawEvent, cancellationToken);
        }
        catch (Exception ex) when (ex is not WyomingException)
        {
            throw new WyomingSerializerException("An unexpected error occurred while serializing a Wyoming protocol message.", ex);
        }
    }
}
