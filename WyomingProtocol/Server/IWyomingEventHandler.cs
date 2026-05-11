using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Server;

public interface IWyomingEventHandler
{
    Task HandleAsync(IWyomingEvent wyomingEvent, IWyomingRequestContext context, CancellationToken cancellationToken);
}
