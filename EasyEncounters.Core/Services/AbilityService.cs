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
        copyTarget.TargetAreaSize = copySource.TargetAreaSize;
        copyTarget.SpellLevel = copySource.SpellLevel;
        copyTarget.TimeDuration = copySource.TimeDuration;
        copyTarget.TimeDurationType = copySource.TimeDurationType;
        copyTarget.SaveType = copySource.SaveType;
        copyTarget.MagicSchool = copySource.MagicSchool;
        copyTarget.TargetDistanceType = copySource.TargetDistanceType;
        copyTarget.Concentration = copySource.Concentration;
    }

    public ActiveAbility CreateActiveAbility(ActiveEncounterCreature activeCreature, Ability ability, int abilityStatValue)
    {
        ActiveAbility activeAbility = new ActiveAbility();
        CopyTo(activeAbility, ability);

        switch (activeAbility.Resolution)
        {
            case ResolutionType.Attack:
                activeAbility.ResolutionValue = abilityStatValue + _baseAttackBonus + activeCreature.ProficiencyBonus;
                break;

            case ResolutionType.SavingThrow:
                activeAbility.ResolutionValue = abilityStatValue + _baseDCBonus + activeCreature.ProficiencyBonus;
                break;

            default:
                activeAbility.ResolutionValue = abilityStatValue;
                break;
        }

        return activeAbility;
    }
}