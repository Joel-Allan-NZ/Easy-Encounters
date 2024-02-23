namespace EasyEncounters.Core.Models.Logs;

public class TurnLog
{
    public TurnLog(ActiveEncounterCreature activeEncounterCreature)
    {
        ActiveTurnCreature = activeEncounterCreature;
        TurnStart = DateTime.Now;
    }

    public ActiveEncounterCreature ActiveTurnCreature
    {
        get; set;
    }

    public List<DamageInstanceLog> DamageInstances
    {
        get; set;
    }

    public DateTime? TurnEnd
    {
        get; set;
    }

    public DateTime TurnStart
    {
        get; set;
    }
}