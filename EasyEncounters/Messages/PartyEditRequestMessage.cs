using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

internal class PartyEditRequestMessage : EventRaisedMessage<PartyViewModel>
{
    public PartyEditRequestMessage(PartyViewModel parameter) : base(parameter)
    {
    }
}