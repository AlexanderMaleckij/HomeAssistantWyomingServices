using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Services;

public interface IWyomingSerializer
{
    Task<IWyomingEvent> DeserializeAsync(Stream stream, CancellationToken cancellationToken = default);

    Task<Stream> SerializeAsync(IWyomingEvent @event, CancellationToken cancellationToken = default);
}
