using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

public class InspectRequestMessage
{
    public InspectRequestMessage(ActiveEncounterCreatureViewModel creature)
    {
        Parameter = creature;
    }

    public ActiveEncounterCreatureViewModel Parameter
    {
        get; private set;
    }
}