namespace EasyEncounters.Messages;

public class EventRaisedMessage<T>
{
    public EventRaisedMessage(T parameter)
    {
        Parameter = parameter;
    }

    public T Parameter
    {
        get; set;
    }
}