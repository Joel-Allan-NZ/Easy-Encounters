using EasyEncounters.Models;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

public class AbilityDamageRequestMessage
{
    public AbilityDamageRequestMessage(ObservableActiveAbility? ability, ObservableActiveEncounterCreature source)
    {
        Ability = ability;
        SourceCreature = source;
    }

    public ObservableActiveAbility? Ability
    {
        get; private set;
    }

    public ObservableActiveEncounterCreature SourceCreature
    {
        get; private set;
    }
}