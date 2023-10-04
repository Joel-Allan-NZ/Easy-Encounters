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
public class CreatureService : ICreatureService
{
    private const int _baseStatZeroBonusValue = 10;
    private IAbilityService _abilityService;
    public CreatureService(IAbilityService abilityService)
    {
        _abilityService = abilityService;
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
            //CreatureAttributeType.None => throw new ArgumentException("CreatureAttributeType must not be None"),
            //_ => throw new ArgumentException($"Invalid CreatureAttributeType: {creatureAttributeType}")
        };
    }

    public int GetAttributeBonusValue(Creature creature, CreatureAttributeType creatureAttributeType)
    {
        return (GetAttributeTypeValue(creature, creatureAttributeType) - _baseStatZeroBonusValue)/2;
    }

    public void CopyTo(Creature target, Creature source)
    {
        //TODO: investigate if necessary. For now, just hacky work around with abilities doubling.
        var placeHolder = new List<Ability>(source.Abilities);

        target.Abilities.Clear();

        foreach (Ability ability in  placeHolder)
        {
            //copy spells as is, but clone other abilities
            if (ability.SpellLevel != SpellLevel.NotASpell)
                target.Abilities.Add(ability);
            else
            {
                var newAbility = new Ability();
                _abilityService.CopyTo(newAbility, ability);
                target.Abilities.Add(ability);
            }
        }

        target.Name = source.Name;
        target.InitiativeAdvantage = source.InitiativeAdvantage;
        target.InitiativeBonus = source.InitiativeBonus;
        target.DMControl = source.DMControl;
        target.Description = source.Description;
        target.LevelOrCR = source.LevelOrCR;
        target.Immunity = source.Immunity;
        target.Vulnerability = source.Vulnerability;
        target.Resistance = source.Resistance;
        target.Hyperlink = source.Hyperlink;
        target.MaxHP = source.MaxHP;
        target.MaxHPString = source.MaxHPString;
        target.AC = source.AC;
        target.MaxLegendaryResistance = source.MaxLegendaryResistance;
        target.MaxLegendaryActions = source.MaxLegendaryActions;
        target.ConditionImmunities = source.ConditionImmunities;
        target.AttackDescription = source.AttackDescription;
        target.Strength = source.Strength;
        target.StrengthSave = source.StrengthSave;
        target.Dexterity = source.Dexterity;
        target.DexteritySave = source.DexteritySave;
        target.Constitution = source.Constitution;
        target.ConstitutionSave = source.ConstitutionSave;
        target.Intelligence = source.Intelligence;
        target.IntelligenceSave = source.IntelligenceSave;
        target.Wisdom = source.Wisdom;
        target.WisdomSave = source.WisdomSave;
        target.Charisma = source.Charisma;
        target.ProficiencyBonus = source.ProficiencyBonus;
        target.CharismaSave = source.CharismaSave;
        target.Movement = source.Movement;
        target.SpellStat = source.SpellStat;
        target.Features = source.Features ?? "";

        if (target.SpellSlots != source.SpellSlots)
        {
            target.SpellSlots.Clear();
            foreach (var kvp in source.SpellSlots)
            {
                target.SpellSlots.Add(kvp.Key, kvp.Value);
            }
        }
        
    }
}
