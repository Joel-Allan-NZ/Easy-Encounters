using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

internal class EncounterCopyRequestMessage : EventRaisedMessage<EncounterViewModel>
{
    public EncounterCopyRequestMessage(EncounterViewModel parameter) : base(parameter)
    {
    }
}