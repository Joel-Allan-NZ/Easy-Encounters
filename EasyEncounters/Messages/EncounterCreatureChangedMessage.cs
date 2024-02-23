using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

public class EncounterCreatureChangedMessage
{
    public EncounterCreatureChangedMessage(IList<ActiveEncounterCreatureViewModel> creatures)
    {
        Creatures = creatures;
    }

    public IList<ActiveEncounterCreatureViewModel> Creatures
    {
        get; private set;
    }
}