using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Models;
using EasyEncounters.Models;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages
{
    public class AbilityDamageRequestMessage
    {
        public ObservableActiveAbility? Ability
        {
            get; private set;
        }

        public ActiveEncounterCreatureViewModel SourceCreature
        {
            get; private set;
        }

        public AbilityDamageRequestMessage(ObservableActiveAbility? ability, ActiveEncounterCreatureViewModel source)
        {
            Ability = ability;
            SourceCreature = source;
        }
    }
}
