using System.Text.Json;
using WyomingProtocol.Exceptions;
using WyomingProtocol.Extensions;
using WyomingProtocol.Models;
using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Services;

internal sealed class RawWyomingSerializer : IRawWyomingSerializer
{
    public async Task<RawWyomingEvent> DeserializeAsync(Stream stream, CancellationToken cancellationToken)
    {
        try
        {
            var header = await ReadHeaderAsync(stream, cancellationToken);
            var mergedData = await GetMergedDataAsync(stream, header.DataLength, header.Data, cancellationToken);
            var payload = await GetPayloadAsync(stream, header.PayloadLength, cancellationToken);

            return new RawWyomingEvent
            {
                Type = header.Type,
                Version = header.Version,
                Data = mergedData,
                Payload = payload,
            };
        }
        catch (Exception ex) when (ex is not WyomingException)
        {
            throw new WyomingSerializerException("An unexpected error occurred while deserializing raw Wyoming protocol message.", ex);
        }
    }

    public async Task<Stream> SerializeAsync(RawWyomingEvent @event, CancellationToken cancellationToken)
    {
        try
        {
            var serializedData = @event.Data is not null
                ? JsonSerializer.SerializeToUtf8Bytes(@event.Data, WyomingSerializerJsonContext.Default.NullableJsonElement)
                : null;

            var header = new RawWyomingEventHeader
            {
                Type = @event.Type,
                Version = @event.Version,
                DataLength = serializedData?.Length,
                PayloadLength = @event.Payload?.Length
            };

            var memoryStream = new MemoryStream();

            await JsonSerializer.SerializeAsync(memoryStream, header, WyomingSerializerJsonContext.Default.RawWyomingEventHeader, cancellationToken);
            memoryStream.WriteByte((byte)'\n');

            if (serializedData is not null)
            {
                await memoryStream.WriteAsync(serializedData, cancellationToken);
            }

            if (@event.Payload is not null)
            {
                await memoryStream.WriteAsync(@event.Payload, cancellationToken);
            }

            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }
        catch (Exception ex) when (ex is not WyomingException)
        {
            throw new WyomingSerializerException("An unexpected error occurred while serializing raw Wyoming protocol message.", ex);
        }
    }

    private static async ValueTask<RawWyomingEventHeader> ReadHeaderAsync(Stream stream, CancellationToken cancellationToken)
    {
        using var memoryStream = new MemoryStream();

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var buffer = new byte[1];

            var bytesRead = await stream.ReadAsync(buffer, cancellationToken);

            if (bytesRead == 0)
            {
                if (memoryStream.Length == 0)
                {
                    throw new WyomingStreamEmptyException("Wyoming message header parsing error: there is no data available in the stream.");
                }

                throw new WyomingUnexpectedEndOfStreamException("Wyoming message header parsing error: end of the stream has been reached before a new line character was found.");
            }

            var streamByte = buffer[0];

            if (streamByte == '\n')
            {
                break;
            }

            memoryStream.WriteByte(streamByte);
        }

        memoryStream.Seek(0, SeekOrigin.Begin);

        var header = await JsonSerializer.DeserializeAsync(memoryStream, WyomingSerializerJsonContext.Default.RawWyomingEventHeader, cancellationToken);

        if (header is null)
        {
            throw new WyomingZeroLengthHeaderException("Wyoming protocol message parsing error: received header with zero length.");
        }

        return header;
    }

    private static async Task<JsonElement?> GetMergedDataAsync(Stream stream, int? dataLength, JsonElement? headerData, CancellationToken cancellationToken)
    {
        if (!dataLength.HasValue || dataLength.Value < 1)
        {
            return headerData;
        }

        var additionalData = new byte[dataLength.Value];

        await stream.ReadExactlyAsync(additionalData, 0, additionalData.Length, cancellationToken);

        var additionalDataJson = JsonElement.Parse(additionalData);
        var mergedData = headerData is null
            ? additionalDataJson
            : headerData.Value.Merge(additionalDataJson);

        return mergedData;
    }

    private static async Task<byte[]?> GetPayloadAsync(Stream stream, int? payloadLength, CancellationToken cancellationToken)
    {
        if (!payloadLength.HasValue || payloadLength.Value < 1)
        {
            return null;
        }

        var payload = new byte[payloadLength.Value];

        await stream.ReadExactlyAsync(payload, 0, payload.Length, cancellationToken);

        return payload;
    }
}
