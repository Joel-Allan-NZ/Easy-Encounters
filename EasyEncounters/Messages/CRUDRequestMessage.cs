using EasyEncounters.Models;

namespace EasyEncounters.Messages;

internal class CRUDRequestMessage<T>
{
    public CRUDRequestMessage(T parameter, CRUDRequestType requestType)
    {
        Parameter = parameter;
        RequestType = requestType;
    }

    public T Parameter
    {
        get; set;
    }

    public CRUDRequestType RequestType
    {
        get; set;
    }
}