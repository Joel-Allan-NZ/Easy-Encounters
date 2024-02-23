using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

public class CreatureEditRequestMessage : EventRaisedMessage<CreatureViewModel>
{
    public CreatureEditRequestMessage(CreatureViewModel parameter) : base(parameter)
    {
    }
}