using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Services;
public class AbilityService : IAbilityService
{
    private const int _baseAttackBonus = 10;
    private const int _baseDCBonus = 8;

    public AbilityService()
    {
    }

    public ActiveAbility CreateActiveAbility(ActiveEncounterCreature creature, Ability ability, int abilityStatValue)
    {
        ActiveAbility activeAbility = new ActiveAbility();
        CopyTo(activeAbility, ability);

        //var resStatValue = _creatureService.GetAttributeTypeValue(creature, activeAbility.ResolutionStat);
        if (activeAbility.Resolution == Models.Enums.ResolutionType.Attack)
        {
            activeAbility.ResolutionValue = abilityStatValue + _baseAttackBonus + creature.ProficiencyBonus;
        }
        else if (activeAbility.Resolution == Models.Enums.ResolutionType.SavingThrow)
        {
            activeAbility.ResolutionValue = abilityStatValue + _baseDCBonus + creature.ProficiencyBonus;
        }
        else
            activeAbility.ResolutionValue = abilityStatValue;

        return activeAbility;
    }

    public void CopyTo(Ability copyTarget, Ability copySource)
    {
        copyTarget.ActionSpeed = copySource.ActionSpeed;
        copyTarget.CastingComponents = copySource.CastingComponents;
        copyTarget.ResolutionStat = copySource.ResolutionStat;
        copyTarget.DamageTypes = copySource.DamageTypes;
        copyTarget.EffectDescription = copySource.EffectDescription;
        copyTarget.MaterialCost = copySource.MaterialCost;
        copyTarget.Name = copySource.Name;
        copyTarget.Resolution = copySource.Resolution;
        copyTarget.TargetAreaType = copySource.TargetAreaType;
        copyTarget.TargetCount = copySource.TargetCount;
        copyTarget.TargetDistance = copySource.TargetDistance;
        copyTarget.TargetSize = copySource.TargetSize;
        copyTarget.SpellLevel = copySource.SpellLevel;
        copyTarget.TimeDuration = copySource.TimeDuration;
        copyTarget.TimeDurationType = copySource.TimeDurationType;
        copyTarget.SaveType = copySource.SaveType;
        copyTarget.MagicSchool = copySource.MagicSchool;
        copyTarget.TargetDistanceType = copySource.TargetDistanceType;
        copyTarget.Concentration = copySource.Concentration;
       
    }
}
