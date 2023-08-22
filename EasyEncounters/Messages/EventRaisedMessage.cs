using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEncounters.Messages;
public class EventRaisedMessage<T>
{
    public T Parameter
    {
        get; set;
    }

    public EventRaisedMessage(T parameter)
    {
        Parameter = parameter;
    }

}
