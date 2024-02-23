using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

internal class EncounterDeleteRequestMessage : EventRaisedMessage<EncounterViewModel>
{
    public EncounterDeleteRequestMessage(EncounterViewModel parameter) : base(parameter)
    {
    }
}