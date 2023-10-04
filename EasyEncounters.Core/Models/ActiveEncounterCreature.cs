using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Models;
public class ActiveEncounterCreature : Creature
{
    public int CurrentHP
    {
        get; set;
    }

    public int EncounterMaxHP
    {
        get; set;
    }

    /// <summary>
    /// The amount of temp HP a creature has. Damage is 'taken' from this before being added to total health damage taken.
    /// </summary>
    public int TempHP
    {
        get; set;
    }

    /// <summary>
    /// An idea for this active creature encounter instance; for persistance of an active combat in case of crashes.
    /// </summary>
    public Guid EncounterID
    {
        get; set;
    }

    /// <summary>
    /// The name of this specific instanced creature within the encounter (for tracking of who is where)
    /// </summary>
    public string EncounterName
    {
        get; set;
    }

    /// <summary>
    /// The creature's initiative count
    /// </summary>
    public int Initiative
    {
        get; set;
    }

    /// <summary>
    /// Any notes that are relative to the creature's current doings in this encounter. Good way to track conditions + their endings etc.
    /// </summary>
    public string Notes
    {
        get; set;
    }

    /// <summary>
    /// Tracks whether or not the creature has a reaction available.
    /// </summary>
    public bool Reaction
    {
        get; set;
    }

    /// <summary>
    /// Is the creature dead? If not then it shouldn't be in the turn order.
    /// </summary>
    public bool Dead
    {
        get; set;
    }

    /// <summary>
    /// Whether or not the creature is currently concentrating.
    /// </summary>
    public bool Concentrating
    {
        get; set;
    }

    /// <summary>
    /// How many legendary actions are currently available.
    /// </summary>
    public int CurrentLegendaryActions
    {
        get; set;
    }

    /// <summary>
    /// How many legendary resistance charges are unused.
    /// </summary>
    public int CurrentLegendaryResistance
    {
        get; set;
    }

    public Condition ActiveConditions
    {
        get; set;
    }

    public List<ActiveAbility> ActiveAbilities
    {
        get; set;
    } = new();

    public ActiveEncounterCreature(Creature creature)
    {
        //ActiveAbilities = new();

        //InitiativeBonus = creature.InitiativeBonus;
        //InitiativeAdvantage = creature.InitiativeAdvantage;
        //DMControl = creature.DMControl;
        //DexBonus = creature.DexBonus;
        Name = creature.Name;
        EncounterName = creature.Name ?? "Unnamed Creature"; //placeholder name, expect this to be overwritten in an active encounter
        //Resistance = creature.Resistance;
        //Immunity = creature.Immunity;
        //Vulnerability = creature.Vulnerability;
        Dead = false;
        EncounterID = Guid.NewGuid();
        Reaction = true;
        Notes = ""+creature.Description;
        //Hyperlink = creature.Hyperlink;
        //MaxHP = creature.MaxHP;
        //CurrentHP = creature.MaxHP;
        Concentrating = false;
        Initiative = -100; //impossible to reach, purely for confirming this creature hasn't rolled initiative yet.
        CurrentLegendaryActions = creature.MaxLegendaryActions;
        CurrentLegendaryResistance = creature.MaxLegendaryResistance;
        //MaxHPString = creature.MaxHPString;
        //AC = creature.AC;
        ActiveConditions = Condition.None;
        //ProficiencyBonus = creature.ProficiencyBonus;
        //AttackDescription = creature.AttackDescription;
        //Strength = creature.Strength;
        //StrengthSave = creature.StrengthSave;
        //Dexterity = creature.Dexterity;
        //DexteritySave = creature.DexteritySave;
        //Constitution = creature.Constitution;
        //ConstitutionSave = creature.ConstitutionSave;
        //Intelligence = creature.Intelligence;
        //IntelligenceSave = creature.IntelligenceSave;
        //Wisdom = creature.Wisdom;
        //WisdomSave = creature.WisdomSave;
        //Charisma = creature.Charisma;
        //CharismaSave = creature.CharismaSave;
        //Movement = creature.Movement;
        //Features = creature.Features;
        SpellSlots = new Dictionary<int, int>();

        foreach(var kvp in creature.SpellSlots)
        {
            SpellSlots.Add(kvp.Key, kvp.Value);
        }

    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ActiveEncounterCreature()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {

    }

}
