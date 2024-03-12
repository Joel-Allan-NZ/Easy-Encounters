using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

internal class AddTargetCreatureRequestMessage : EventRaisedMessage<ObservableActiveEncounterCreature>
{
    public AddTargetCreatureRequestMessage(ObservableActiveEncounterCreature parameter) : base(parameter)
    {
    }
}