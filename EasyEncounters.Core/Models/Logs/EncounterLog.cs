using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEncounters.Core.Models.Logs;
public class EncounterLog
{
    public List<TurnLog> Turns
    {
        get; set; 
    }
    public EncounterLog()
    {
        Turns = new List<TurnLog>();
    }
}
