using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

internal class AddTargetCreatureRequestMessage : EventRaisedMessage<ActiveEncounterCreatureViewModel>
{
    public AddTargetCreatureRequestMessage(ActiveEncounterCreatureViewModel parameter) : base(parameter)
    {
    }
}