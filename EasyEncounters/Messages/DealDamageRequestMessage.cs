using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

internal class DealDamageRequestMessage
{
    public DealDamageRequestMessage(List<DamageCreatureViewModel> targets, ActiveEncounterCreature sourceCreature, int baseDamage, DamageType damageType)
    {
        Targets = targets;
        SourceCreature = sourceCreature;
        BaseDamage = baseDamage;
        DamageType = damageType;
    }

    public int BaseDamage
    {
        get;
    }

    public DamageType DamageType
    {
        get;
    }

    public ActiveEncounterCreature SourceCreature
    {
        get;
    }

    public List<DamageCreatureViewModel> Targets
    {
        get;
    }
}