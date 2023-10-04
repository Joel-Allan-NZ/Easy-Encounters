using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Models;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages
{
    public class AddAbilityRequestMessage
    {
        public AbilityViewModel AbilityViewModel
        {
            get; private set;
        }
        public AddAbilityRequestMessage(AbilityViewModel abilityViewModel)
        {
            AbilityViewModel = abilityViewModel;
        }
    }
}
