using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Models.Logs;
public class CharacterData
{
    public Dictionary<DamageType, double> DamageDealt
    {
        get; set;
    }

    public Dictionary<DamageType, double> DamageTaken
    {
        get; set;
    }

    public double TurnSecondsTaken
    {
        get; set;
    }

    public CharacterData()
    {
        DamageDealt = new Dictionary<DamageType, double>();
        DamageTaken = new Dictionary<DamageType, double>();
        TurnSecondsTaken = 0;
    }

    //not implemented/require major changes: TEMP HP granted, healing done, CC duration, times downed, enemies killed.
}
