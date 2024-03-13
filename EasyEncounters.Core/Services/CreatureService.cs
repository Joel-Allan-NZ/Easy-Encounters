using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Services;

public class CreatureService : ICreatureService
{
    private const int _baseStatZeroBonusValue = 10;
    private readonly IAbilityService _abilityService;

    public CreatureService(IAbilityService abilityService)
    {
        _abilityService = abilityService;
    }

    public Creature Create()
    {
        return new Creature()
        {
            Abilities = new(),
            SpellSlots = new Dictionary<int, int>(),
            Id = Guid.NewGuid()
        };
    }

    public Creature DeepCopy(Creature source)
    {
        var clone = new Creature(source);

        clone.Abilities.Clear();

        foreach (Ability ability in source.Abilities)
        {
            //copy spells as is (as they're effectively just references), but clone other abilities
            if (ability.SpellLevel != SpellLevel.NotASpell)
                clone.Abilities.Add(ability);
            else
            {
                var newAbility = new Ability();
                _abilityService.CopyTo(newAbility, ability);
                clone.Abilities.Add(ability);
            }
        }
        return clone;
    }

    public int GetAttributeBonusValue(Creature creature, CreatureAttributeType creatureAttributeType)
    {
        return (GetAttributeTypeValue(creature, creatureAttributeType) - _baseStatZeroBonusValue) / 2;
    }

    public int GetAttributeTypeValue(Creature creature, CreatureAttributeType creatureAttributeType)
    {
        return creatureAttributeType switch
        {
            CreatureAttributeType.Strength => creature.Strength,
            CreatureAttributeType.Dexterity => creature.Dexterity,
            CreatureAttributeType.Constitution => creature.Constitution,
            CreatureAttributeType.Intelligence => creature.Intelligence,
            CreatureAttributeType.Wisdom => creature.Wisdom,
            CreatureAttributeType.Charisma => creature.Charisma,
            _ => 0
        };
    }

    public CreatureSkillLevel GetSkillProficiencyLevel(Creature creature, CreatureSkills skill)
    {
        if (creature.Expertise.HasFlag(skill))
        {
            return CreatureSkillLevel.Expertise;
        }
        if (creature.Proficient.HasFlag(skill))
        {
            return CreatureSkillLevel.Proficient;
        }
        if (creature.HalfProficient.HasFlag(skill))
        {
            return CreatureSkillLevel.HalfProficient;
        }
        return CreatureSkillLevel.None;
    }

    public int GetSkillBonusTotal(Creature creature, CreatureSkills skill, CreatureSkillLevel proficiencyLevel)
    {
        var statBonus = GetAttributeBonusValue(creature, GetBaseSkillAttributeType(skill));

        var proficiencyBonus = (int)proficiencyLevel / 2 * creature.ProficiencyBonus;

        return statBonus + proficiencyBonus;
    }

    private static CreatureAttributeType GetBaseSkillAttributeType(CreatureSkills skill)
    {
        return skill switch
        {
            CreatureSkills.Acrobatics => CreatureAttributeType.Dexterity,
            CreatureSkills.AnimalHandling => CreatureAttributeType.Wisdom,
            CreatureSkills.Arcana => CreatureAttributeType.Intelligence,
            CreatureSkills.Athletics => CreatureAttributeType.Strength,
            CreatureSkills.Deception => CreatureAttributeType.Charisma,
            CreatureSkills.History => CreatureAttributeType.Intelligence,
            CreatureSkills.Insight => CreatureAttributeType.Wisdom,
            CreatureSkills.Intimidation => CreatureAttributeType.Charisma,
            CreatureSkills.Investigation => CreatureAttributeType.Intelligence,
            CreatureSkills.Medicine => CreatureAttributeType.Wisdom,
            CreatureSkills.Nature => CreatureAttributeType.Intelligence,
            CreatureSkills.Perception => CreatureAttributeType.Wisdom,
            CreatureSkills.Performance => CreatureAttributeType.Charisma,
            CreatureSkills.Persuasion => CreatureAttributeType.Charisma,
            CreatureSkills.Religion => CreatureAttributeType.Intelligence,
            CreatureSkills.SleightOfHand => CreatureAttributeType.Dexterity,
            CreatureSkills.Stealth => CreatureAttributeType.Dexterity,
            CreatureSkills.Survival => CreatureAttributeType.Wisdom,
            CreatureSkills.None => CreatureAttributeType.None,
            _ => throw new ArgumentException($"{skill} is not a valid skill")
        };
    }
}