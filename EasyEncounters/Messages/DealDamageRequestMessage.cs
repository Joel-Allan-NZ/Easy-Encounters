using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;
internal class DealDamageRequestMessage
{
    public List<DamageCreatureViewModel> Targets
    {
        get;
    }

    public ActiveEncounterCreature SourceCreature
    {
        get; 
    }

    public int BaseDamage
    {
        get; 
    }

    public DamageType DamageType
    {
        get; 
    }
    public DealDamageRequestMessage(List<DamageCreatureViewModel> targets, ActiveEncounterCreature sourceCreature, int baseDamage, DamageType damageType)
    {
        Targets = targets;
        SourceCreature = sourceCreature;
        BaseDamage = baseDamage;
        DamageType = damageType;
    }
}
