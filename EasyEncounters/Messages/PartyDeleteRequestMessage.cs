using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

internal class PartyDeleteRequestMessage : EventRaisedMessage<PartyViewModel>
{
    public PartyDeleteRequestMessage(PartyViewModel parameter) : base(parameter)
    {
    }
}