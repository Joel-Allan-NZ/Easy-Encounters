using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Models;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Models;
internal class EncounterDamageTabData
{
    internal ActiveEncounter ActiveEncounter
    {
    get; set; } 

    internal ActiveEncounterCreatureViewModel Source
    {
        get; set;
    }

    internal ObservableActiveAbility? Ability
    {
        get; set;
    }

    internal IList<ActiveEncounterCreatureViewModel> Targets
    {
        get; set;
    }

    public EncounterDamageTabData(ActiveEncounter activeEncounter, ActiveEncounterCreatureViewModel source, 
        IList<ActiveEncounterCreatureViewModel> targets, ObservableActiveAbility? ability = null)
    {
        ActiveEncounter = activeEncounter;
        Source = source;
        Targets = targets;
        Ability = ability
            ;
    }
}
