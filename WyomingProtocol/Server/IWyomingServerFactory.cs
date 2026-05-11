namespace WyomingProtocol.Server;

public interface IWyomingServerFactory
{
    IWyomingServer Create(string handlerKey);
}
