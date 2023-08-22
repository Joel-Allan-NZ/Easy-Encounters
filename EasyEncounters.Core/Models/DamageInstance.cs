using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Models;
public class DamageInstance
{
    public ActiveEncounterCreature Target;

    public DamageType DamageType;

    public ActiveEncounterCreature Source;

    public int BaseDamageValue;

    public DamageVolume DamageVolume;

    public DamageInstance(ActiveEncounterCreature target, ActiveEncounterCreature source, DamageType damageType, DamageVolume damageVolume, int baseDamageValue)
    {
        Target = target;
        Source = source;
        DamageType = damageType;
        DamageVolume = damageVolume;
        BaseDamageValue = baseDamageValue;
    }
}
