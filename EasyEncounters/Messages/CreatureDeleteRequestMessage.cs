using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

internal class CreatureDeleteRequestMessage : EventRaisedMessage<CreatureViewModel>
{
    public CreatureDeleteRequestMessage(CreatureViewModel parameter) : base(parameter)
    {
    }
}