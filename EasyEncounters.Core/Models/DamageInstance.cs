using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Models;

public class DamageInstance
{
    public int BaseDamageValue;
    public DamageType DamageType;
    public DamageVolume DamageVolume;
    public ActiveEncounterCreature Source;
    public ActiveEncounterCreature Target;

    public DamageInstance(ActiveEncounterCreature target, ActiveEncounterCreature source, DamageType damageType, DamageVolume damageVolume, int baseDamageValue)
    {
        Target = target;
        Source = source;
        DamageType = damageType;
        DamageVolume = damageVolume;
        BaseDamageValue = baseDamageValue;
    }
}