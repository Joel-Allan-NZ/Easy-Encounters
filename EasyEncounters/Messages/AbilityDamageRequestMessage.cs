using EasyEncounters.Models;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

public class AbilityDamageRequestMessage
{
    public AbilityDamageRequestMessage(ObservableActiveAbility? ability, ActiveEncounterCreatureViewModel source)
    {
        Ability = ability;
        SourceCreature = source;
    }

    public ObservableActiveAbility? Ability
    {
        get; private set;
    }

    public ActiveEncounterCreatureViewModel SourceCreature
    {
        get; private set;
    }
}