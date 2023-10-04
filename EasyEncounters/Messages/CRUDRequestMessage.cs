using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Models;

namespace EasyEncounters.Messages;
internal class CRUDRequestMessage<T>
{
    public T Parameter
    {
        get; set;
    }

    public CRUDRequestType RequestType
    {
        get; set; 
    }

    public CRUDRequestMessage(T parameter, CRUDRequestType requestType)
    {
        Parameter = parameter;
        RequestType = requestType;
    }
}
