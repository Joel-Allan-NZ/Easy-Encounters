using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

public class InspectRequestMessage
{
    public InspectRequestMessage(ObservableActiveEncounterCreature creature)
    {
        Parameter = creature;
    }

    public ObservableActiveEncounterCreature Parameter
    {
        get; private set;
    }
}