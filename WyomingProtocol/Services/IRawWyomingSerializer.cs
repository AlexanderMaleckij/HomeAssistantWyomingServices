using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Services;

public interface IRawWyomingSerializer
{
    Task<RawWyomingEvent> DeserializeAsync(Stream stream, CancellationToken cancellationToken);

    Task<Stream> SerializeAsync(RawWyomingEvent @event, CancellationToken cancellationToken);
}
