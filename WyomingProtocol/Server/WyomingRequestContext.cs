using System.Net.Sockets;
using WyomingProtocol.Models.Events;
using WyomingProtocol.Services;

namespace WyomingProtocol.Server
{
    internal sealed class WyomingRequestContext : IWyomingRequestContext
    {
        private readonly TcpClient _tcpClient;
        private readonly IWyomingSerializer _wyomingSerializer;

        internal bool IsClosedConnection { get; private set; }

        public WyomingRequestContext(
            TcpClient tcpClient,
            IWyomingSerializer wyomingSerializer)
        {
            ArgumentNullException.ThrowIfNull(tcpClient);
            ArgumentNullException.ThrowIfNull(wyomingSerializer);

            _tcpClient = tcpClient;
            _wyomingSerializer = wyomingSerializer;
        }

        public void CloseConnection()
        {
            _tcpClient.Close();
            IsClosedConnection = true;
        }

        public async Task RespondAsync(IWyomingEvent response, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(response);

            var responseStream = await _wyomingSerializer.SerializeAsync(response, cancellationToken);

            await responseStream.CopyToAsync(_tcpClient.GetStream(), cancellationToken);
        }
    }
}
