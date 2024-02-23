using EasyEncounters.Core.Models;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Models;

internal class EncounterDamageTabData
{
    public EncounterDamageTabData(ActiveEncounter activeEncounter, ActiveEncounterCreatureViewModel source,
        IList<ActiveEncounterCreatureViewModel> targets, ObservableActiveAbility? ability = null)
    {
        ActiveEncounter = activeEncounter;
        Source = source;
        Targets = targets;
        Ability = ability
            ;
    }

    internal ObservableActiveAbility? Ability
    {
        get; set;
    }

    internal ActiveEncounter ActiveEncounter
    {
        get; set;
    }

    internal ActiveEncounterCreatureViewModel Source
    {
        get; set;
    }

    internal IList<ActiveEncounterCreatureViewModel> Targets
    {
        get; set;
    }
}