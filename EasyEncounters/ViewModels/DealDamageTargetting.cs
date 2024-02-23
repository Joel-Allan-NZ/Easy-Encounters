using EasyEncounters.Core.Models;

namespace EasyEncounters.ViewModels
{
    internal class DealDamageTargetting
    {
        internal ActiveEncounter Encounter;
        internal ActiveEncounterCreatureViewModel Source;
        internal IEnumerable<ActiveEncounterCreatureViewModel> Targets;

        public DealDamageTargetting(ActiveEncounter encounter, ActiveEncounterCreatureViewModel source, IEnumerable<ActiveEncounterCreatureViewModel> targets)
        {
            Source = source;
            Targets = targets;
            Encounter = encounter;
        }
    }
}