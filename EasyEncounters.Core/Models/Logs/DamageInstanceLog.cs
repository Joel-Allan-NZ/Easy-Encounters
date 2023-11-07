using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEncounters.Core.Models.Logs;
public class DamageInstanceLog
{
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

    public DamageInstanceLog(DamageInstance damageInstance)
    {
        DamageInstance = damageInstance;
        Time = DateTime.Now;
    }
}
