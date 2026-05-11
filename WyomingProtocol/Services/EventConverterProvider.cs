using Microsoft.Extensions.DependencyInjection;
using WyomingProtocol.Converters;

namespace WyomingProtocol.Services
{
    internal sealed class EventConverterProvider : IEventConverterProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public EventConverterProvider(IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);

            _serviceProvider = serviceProvider;
        }

        public IEventConverter? GetSerializer(string eventType)
        {
            return _serviceProvider.GetKeyedService<IEventConverter>(eventType);
        }
    }
}
