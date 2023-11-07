using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEncounters.Core.Models.Logs;
public class TurnLog
{
    public DateTime TurnStart
    {
        get; set;
    }

    public DateTime? TurnEnd
    {
        get; set;
    }

    public List<DamageInstanceLog> DamageInstances
    {
        get; set; 
    }

    public ActiveEncounterCreature ActiveTurnCreature
    {
        get; set;
    }

    public TurnLog(ActiveEncounterCreature activeEncounterCreature)
    {
        ActiveTurnCreature = activeEncounterCreature;
        TurnStart = DateTime.Now;
    }

}
