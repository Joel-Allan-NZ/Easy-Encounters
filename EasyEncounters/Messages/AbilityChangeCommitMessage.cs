using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Models;
using EasyEncounters.Models;

namespace EasyEncounters.Messages
{
    public class AbilityChangeCommitMessage
    {
        public Ability Ability
        {
            get; private set;
        }

        public AbilityChangeCommitMessage(Ability ability)
        {
            Ability = ability;
        }
    }
}
