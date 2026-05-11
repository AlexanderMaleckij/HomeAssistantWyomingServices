using WyomingProtocol.Converters;

namespace WyomingProtocol.Services;

internal interface IEventConverterProvider
{
    IEventConverter? GetSerializer(string eventType);
}
