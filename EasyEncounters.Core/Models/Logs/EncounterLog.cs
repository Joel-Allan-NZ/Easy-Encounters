namespace EasyEncounters.Core.Models.Logs;

public class EncounterLog
{
    public EncounterLog()
    {
        Turns = new List<TurnLog>();
    }

    public List<TurnLog> Turns
    {
        get; set;
    }
}