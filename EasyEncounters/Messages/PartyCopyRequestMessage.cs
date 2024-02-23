using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

internal class PartyCopyRequestMessage : EventRaisedMessage<PartyViewModel>
{
    public PartyCopyRequestMessage(PartyViewModel parameter) : base(parameter)
    {
    }
}