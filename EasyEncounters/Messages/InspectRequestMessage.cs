using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages
{
    public class InspectRequestMessage
    {
        public ActiveEncounterCreatureViewModel Parameter
        {
            get; private set;
        }
        public InspectRequestMessage(ActiveEncounterCreatureViewModel creature)
        {
            Parameter = creature;
        }
    }
}
