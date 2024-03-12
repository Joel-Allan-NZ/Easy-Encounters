using EasyEncounters.Core.Models;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Models;

internal class EncounterDamageTabData
{
    public EncounterDamageTabData(ActiveEncounter activeEncounter, ObservableActiveEncounterCreature source,
        IList<ObservableActiveEncounterCreature> targets, ObservableActiveAbility? ability = null)
    {
        ActiveEncounter = activeEncounter;
        Source = source;
        Targets = targets;
        Ability = ability;
    }

    internal ObservableActiveAbility? Ability
    {
        get; set;
    }

    internal ActiveEncounter ActiveEncounter
    {
        get; set;
    }

    internal ObservableActiveEncounterCreature Source
    {
        get; set;
    }

    internal IList<ObservableActiveEncounterCreature> Targets
    {
        get; set;
    }
}