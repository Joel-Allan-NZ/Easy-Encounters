using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Models.Logs;

public class CharacterData
{
    public CharacterData()
    {
        DamageDealt = new Dictionary<DamageType, double>();
        DamageTaken = new Dictionary<DamageType, double>();
        TurnSecondsTaken = 0;
    }

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

    //not implemented/require major changes: TEMP HP granted, healing done, CC duration, times downed, enemies killed.
}