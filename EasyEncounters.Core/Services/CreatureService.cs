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
}