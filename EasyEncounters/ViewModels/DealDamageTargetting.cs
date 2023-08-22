using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Models;

namespace EasyEncounters.ViewModels
{
    internal class DealDamageTargetting
    {
        internal IEnumerable<ActiveEncounterCreatureViewModel> Targets;

        internal ActiveEncounterCreatureViewModel Source;

        internal ActiveEncounter Encounter;

        public DealDamageTargetting(ActiveEncounter encounter, ActiveEncounterCreatureViewModel source, IEnumerable<ActiveEncounterCreatureViewModel> targets)
        {
            Source = source;
            Targets = targets;
            Encounter = encounter;
        }
    }
}
