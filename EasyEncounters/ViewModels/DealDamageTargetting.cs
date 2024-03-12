using EasyEncounters.Core.Models;

namespace EasyEncounters.ViewModels
{
    internal class DealDamageTargetting
    {
        internal ActiveEncounter Encounter;
        internal ObservableActiveEncounterCreature Source;
        internal IEnumerable<ObservableActiveEncounterCreature> Targets;

        public DealDamageTargetting(ActiveEncounter encounter, ObservableActiveEncounterCreature source, IEnumerable<ObservableActiveEncounterCreature> targets)
        {
            Source = source;
            Targets = targets;
            Encounter = encounter;
        }
    }
}