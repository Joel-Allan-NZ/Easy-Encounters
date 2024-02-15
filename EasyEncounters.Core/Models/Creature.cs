using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Models.Enums;
#nullable enable

namespace EasyEncounters.Core.Models;
public class Creature : Persistable 
{
    /// <summary>
    /// The flat numeric bonus value to initiative rolls, excluding the bonus from dexterity.
    /// </summary>
    public int InitiativeBonus
    {
        get; set;
    }

    /// <summary>
    /// Whether or not the creature has advantage on initiative rolls
    /// </summary>
    public bool InitiativeAdvantage
    {
        get; set;
    }

    /// <summary>
    /// Whether or not the DM controls this character - allows for smooth/quick initiative rolling of them en mass. Probably can also consider renaming
    /// this to "IsPC" given that the DM typically controls NPC characters.
    /// </summary>
    public bool DMControl
    {
        get; set;
    }


    ///// <summary>
    ///// For initiative tie-breaking.
    ///// </summary>
    //public int DexBonus //TODO: deprecate.
    //{
    //    get; set;
    //}

    /// <summary>
    /// The creature's name. NB this is a statblock wide thing like "orc" not an encounter wide one like "orc1".
    /// </summary>
    public string Name
    {
        get; set;
    }

    /// <summary>
    /// Damage types the creature resists
    /// </summary>
    public DamageType Resistance
    {
        get; set;
    }

    /// <summary>
    /// Damage types the creature is immune to 
    /// </summary>
    public DamageType Immunity
    {
        get; set;
    }

    /// <summary>
    /// Damage types the creature is vulnerable to
    /// </summary>
    public DamageType Vulnerability
    {
        get; set;
    }

    public string Description
    {
        get; set;
    }

    /// <summary>
    /// The level of a player, or challenge rating of a creature.
    /// </summary>
    public double LevelOrCR
    {
        get; set;
    }

    /// <summary>
    /// The XP this single creature would reward if killed.
    /// </summary>
    public int XP
    {
        get; set;
    }

    public string? Hyperlink
    {
        get; set;
    }

    public int MaxHP
    {
        get;
        set;
    }

    /// <summary>
    /// A dice string for rolling for maxHP rather than using a fixed value.
    /// </summary>
    public string MaxHPString
    {
        get; 
        set;
    }

    public int AC
    {
        get; set;
    }

    public int MaxLegendaryActions
    {
        get; set;
    }

    public int MaxLegendaryResistance
    {
        get; set;
    }

    /// <summary>
    ///  The explainer for the number of attacks the creature has, and how they are distributed.
    ///  For example: 'can make 2 spear attacks and 1 bite attack.'
    /// </summary>
    public string AttackDescription
    {
        get; set;
    }

    public Condition ConditionImmunities
    {
        get; set; 
    }

    public int Strength
    {
        get; set;
    }

    public int StrengthSave
    {
        get; set;
    }

    public int Dexterity
    {
        get; set;
    }

    public int DexteritySave
    {
        get; set;
    }

    public int Constitution
    {
        get; set;
    }
    public int ConstitutionSave
    {
        get; set;
    }
    public int Intelligence
    {
        get; set;
    }
    public int IntelligenceSave
    {
        get; set;
    }
    public int Wisdom
    {
        get; set;
    }
    public int WisdomSave
    {
        get; set;
    }
    public int Charisma
    {
        get;set;
    }
    public int CharismaSave
    {
        get; set; 
    }

    public List<Ability> Abilities
    {
        get; set;
    }

    public Dictionary<int, int> SpellSlots
    {
        get; set;
    }

    public int ProficiencyBonus
    {
        get; set;
    }

    /// <summary>
    /// Extra features the creature has, like Magic Resistance, Devil Sight etc
    /// </summary>
    public string Features
    {
        get; set;
    }

    public CreatureAttributeType SpellStat
    {
        get; set;
    }

    public string Movement
    {
        get; set;
    }

    public Creature(string name = "New Creature", /*int dmgTaken =0, int tempHP =0,*/ int initBonus = 0,
        bool initAdv = false, bool dmControl = true, DamageType resistance = DamageType.None,
        DamageType immunity = DamageType.None, DamageType vuln = DamageType.None, string description = "",
        double levelCR = 0.0, string hyper = "https://www.dndbeyond.com/", int maxHP = 0, string maxHPString = "", int aC = 10,
        int legActions = 0, int legRes = 0, string attackDescription = "", Condition conditionImmunities = Condition.None,
        int strength = 10, int strengthSave = 0, int dexterity = 10, int dexteritySave =0, int constitution = 10, int constitutionSave = 0,
        int intelligence = 10, int intelligenceSave = 0, int wisdom = 10, int wisdomSave = 0, int charisma = 10, int charismaSave = 0, 
        Dictionary<int, int>? spellSlots = null, int proficiencyBonus = 0, CreatureAttributeType spellStat = CreatureAttributeType.None, 
        string features = "", string movement = "")
    {
        Name = name;
        Abilities = new List<Ability>();
        //HealthDamageTaken = dmgTaken;
        //TempHP= tempHP;
        InitiativeBonus = initBonus;
        InitiativeAdvantage = initAdv;
        DMControl = dmControl;
        Immunity = immunity;
        Vulnerability = vuln;
        Resistance = resistance;
        Id = Guid.NewGuid();
        Description = description;
        LevelOrCR = levelCR;
        Hyperlink = hyper;
        MaxHP = maxHP;
        MaxHPString = maxHPString;
        AC = aC;
        MaxLegendaryActions = legActions;
        MaxLegendaryResistance = legRes;
        ConditionImmunities = conditionImmunities;
        AttackDescription = attackDescription;
        SpellSlots = spellSlots ?? new Dictionary<int, int>();

        Strength = strength;
        StrengthSave = strengthSave;
        Dexterity = dexterity;
        DexteritySave = dexteritySave;
        Constitution = constitution;
        ConstitutionSave = constitutionSave;
        Intelligence = intelligence;
        IntelligenceSave = intelligenceSave;
        WisdomSave = wisdomSave;
        Wisdom = wisdom;
        Charisma = charisma;
        CharismaSave = charismaSave;
        ProficiencyBonus = proficiencyBonus;

       //AttackStat = attackStat;
        SpellStat = spellStat;
        Features = features;
        Movement=movement;
    }

    public void CopyFrom(Creature other)
    {
        Abilities.Clear();
        foreach(Ability ability in other.Abilities)
        {
            //copy spells as is, but clone other abilities
            if(ability.SpellLevel != SpellLevel.NotASpell)
                Abilities.Add(ability);
            else
            {
                var newAbility = new Ability();
                
            }
        }

        Name = other.Name;
        InitiativeAdvantage = other.InitiativeAdvantage;
        InitiativeBonus = other.InitiativeBonus;
        DMControl = other.DMControl;
        Description = other.Description;
        LevelOrCR = other.LevelOrCR;
        Immunity = other.Immunity;
        Vulnerability = other.Vulnerability;
        Resistance = other.Resistance;
        Hyperlink = other.Hyperlink;
        MaxHP = other.MaxHP;
        MaxHPString = other.MaxHPString;
        AC = other.AC;
        MaxLegendaryResistance = other.MaxLegendaryResistance;
        MaxLegendaryActions = other.MaxLegendaryActions;
        ConditionImmunities = other.ConditionImmunities;
        AttackDescription = other.AttackDescription;
        Strength = other.Strength;
        StrengthSave = other.StrengthSave;
        Dexterity = other.Dexterity;
        DexteritySave = other.DexteritySave;
        Constitution = other.Constitution;
        ConstitutionSave = other.ConstitutionSave;
        Intelligence = other.Intelligence;
        IntelligenceSave = other.IntelligenceSave;
        Wisdom = other.Wisdom;
        WisdomSave = other.WisdomSave;
        Charisma = other.Charisma;
        CharismaSave = other.CharismaSave;
        Movement = other.Movement;
        ProficiencyBonus = other.ProficiencyBonus;

        //AttackStat = other.AttackStat;
        SpellStat = other.SpellStat;
        Features = other.Features;

        SpellSlots.Clear();
        foreach(var kvp in other.SpellSlots)
        {
            SpellSlots.Add(kvp.Key, kvp.Value);
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is Creature)
        {
            return ((Creature)obj).Id == this.Id;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }



}
