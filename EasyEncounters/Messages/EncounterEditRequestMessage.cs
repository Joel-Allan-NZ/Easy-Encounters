using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

internal class EncounterEditRequestMessage : EventRaisedMessage<EncounterViewModel>
{
    public EncounterEditRequestMessage(EncounterViewModel parameter) : base(parameter)
    {
    }
}