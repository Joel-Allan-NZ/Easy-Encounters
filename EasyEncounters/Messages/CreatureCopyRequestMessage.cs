using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

internal class CreatureCopyRequestMessage : EventRaisedMessage<CreatureViewModel>
{
    public CreatureCopyRequestMessage(CreatureViewModel parameter) : base(parameter)
    {
    }
}