namespace EasyEncounters.Core.Models.Logs;

public class DamageInstanceLog
{
    public DamageInstanceLog(DamageInstance damageInstance)
    {
        DamageInstance = damageInstance;
        Time = DateTime.Now;
    }

    public DamageInstance DamageInstance
    {
        get; set;
    }

    public DateTime Time
    {
        get; set;
    }

    public override string ToString()
    {
        return $"[{Time.ToString("HH:mm:ss")}]: {DamageInstance.Source} deals {DamageInstance.DamageVolume} to {DamageInstance.Target}. ";
    }
}