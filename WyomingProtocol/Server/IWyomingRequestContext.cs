using WyomingProtocol.Models.Events;

namespace WyomingProtocol.Server;

public interface IWyomingRequestContext
{
    Task RespondAsync(IWyomingEvent response, CancellationToken cancellationToken = default);

    void CloseConnection();
}
