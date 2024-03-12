using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

public class EncounterCreatureChangedMessage
{
    public EncounterCreatureChangedMessage(IList<ObservableActiveEncounterCreature> creatures)
    {
        Creatures = creatures;
    }

    public IList<ObservableActiveEncounterCreature> Creatures
    {
        get; private set;
    }
}