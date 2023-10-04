using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages
{
    public class EncounterCreatureChangedMessage
    {
        public IList<ActiveEncounterCreatureViewModel> Creatures
        {
            get; private set;
        }
        public EncounterCreatureChangedMessage(IList<ActiveEncounterCreatureViewModel> creatures)
        {
            Creatures = creatures;
        }
    }
}
