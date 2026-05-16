using WyomingProtocol.Converters.Shared;

namespace WyomingProtocol.Services;

internal interface IEventConverterProvider
{
    IEventConverter? GetSerializer(string eventType);
}
