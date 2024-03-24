using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Models;
public class CreatureBase : Persistable
{
    public CreatureBase()
    {
        
    }

    public CreatureBase(string name = "New Creature", int initBonus = 0,
            bool initAdv = false, bool dmControl = true, DamageType resistance = DamageType.None,
            DamageType immunity = DamageType.None, DamageType vuln = DamageType.None, string description = "",
            double levelCR = 0.0, string hyper = "https://www.dndbeyond.com/", int maxHP = 0, string maxHPString = "", int aC = 10,
            int legActions = 0, int legRes = 0, string attackDescription = "", Condition conditionImmunities = Condition.None,
            int strength = 10, int strengthSave = 0, int dexterity = 10, int dexteritySave = 0, int constitution = 10, int constitutionSave = 0,
            int intelligence = 10, int intelligenceSave = 0, int wisdom = 10, int wisdomSave = 0, int charisma = 10, int charismaSave = 0,
            int[] spellSlots = null, int proficiencyBonus = 0, CreatureAttributeType spellStat = CreatureAttributeType.None,
            string features = "", string movement = "", string senses = "", CreatureType creatureType = CreatureType.Aberration, string creatureSubtype = "",
            CreatureAlignment creatureAlignment = CreatureAlignment.Undefined, CreatureSizeClass size = CreatureSizeClass.Medium,
            CreatureSkills notProficient = CreatureSkills.None, CreatureSkills halfProficient = CreatureSkills.None,
            CreatureSkills proficient = CreatureSkills.None, CreatureSkills expertise = CreatureSkills.None, string languages = "", string toolSkills = "")
    {
        Name = name;
        Abilities = new List<Ability>();
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
        SpellSlots = spellSlots ?? new int[9];
        //OldSpellSlots = oldSpellSlots ?? new Dictionary<int, int>();
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

        SpellStat = spellStat;
        Features = features;
        Movement = movement;

        Size = size;
        CreatureType = creatureType;
        Senses = senses;
        CreatureSubtype = creatureSubtype;
        Alignment = creatureAlignment;
        NotProficient = notProficient;
        HalfProficient = halfProficient;
        Proficient = proficient;
        Expertise = expertise;

        Languages = languages;
        ToolSkills = toolSkills;
    }
    public CreatureBase(CreatureBase other)
    {

        Name = other.Name;

        InitiativeBonus = other.InitiativeBonus;
        InitiativeAdvantage = other.InitiativeAdvantage;
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
        ProficiencyBonus = other.ProficiencyBonus;
        CharismaSave = other.CharismaSave;
        Movement = other.Movement;
        SpellStat = other.SpellStat;
        Features = other.Features ?? "";
        Size = other.Size;
        CreatureType = other.CreatureType;
        Senses = other.Senses;
        CreatureSubtype = other.CreatureSubtype;
        Alignment = other.Alignment;
        Id = Guid.NewGuid();

        SpellSlots = (int[])other.SpellSlots.Clone(); //new Dictionary<int, int>(other.SpellSlots);
        Abilities = new List<Ability>(other.Abilities);

        NotProficient = other.NotProficient;
        Proficient = other.Proficient;
        HalfProficient = other.HalfProficient;
        Expertise = other.Expertise;

        ToolSkills = other.ToolSkills;
        Languages = other.Languages;
    }
    public List<Ability> Abilities
    {
        get; set;
    }

    public int AC
    {
        get; set;
    }

    public CreatureAlignment Alignment
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

    public int Charisma
    {
        get; set;
    }

    public int CharismaSave
    {
        get; set;
    }

    public Condition ConditionImmunities
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

    /// <summary>
    /// The Creature's Subtype, if any. For example, a creature with a CreatureType of Humanoid may have a Subtype of Orc.
    /// </summary>
    public string? CreatureSubtype
    {
        get; set;
    }

    public CreatureType CreatureType
    {
        get; set;
    }

    public string Description
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

    /// <summary>
    /// Whether or not the DM controls this character - allows for smooth/quick initiative rolling of them en mass. Probably can also consider renaming
    /// this to "IsPC" given that the DM typically controls NPC characters.
    /// </summary>
    public bool DMControl
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

    /// <summary>
    /// An external hyperlink address for the statblock.
    /// </summary>
    public string Hyperlink
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
    /// Whether or not the creature has advantage on initiative rolls
    /// </summary>
    public bool InitiativeAdvantage
    {
        get; set;
    }

    /// <summary>
    /// The flat numeric bonus value to initiative rolls, excluding the bonus from dexterity.
    /// </summary>
    public int InitiativeBonus
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

    /// <summary>
    /// The level of a player, or challenge rating of a creature.
    /// </summary>
    public double LevelOrCR
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

    public int MaxLegendaryActions
    {
        get; set;
    }

    public int MaxLegendaryResistance
    {
        get; set;
    }

    public string Movement
    {
        get; set;
    }

    /// <summary>
    /// The creature's name. NB this is a statblock wide thing like "orc" not an encounter wide one like "orc1".
    /// </summary>
    public string Name
    {
        get; set;
    }

    public int ProficiencyBonus
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

    public string? Senses
    {
        get; set;
    }

    public CreatureSizeClass Size
    {
        get; set;
    }

    public int[] SpellSlots
    {
        get; set;
    }

    public CreatureAttributeType SpellStat
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

    /// <summary>
    /// Damage types the creature is vulnerable to
    /// </summary>
    public DamageType Vulnerability
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

    /// <summary>
    /// The XP this single creature would reward if killed.
    /// </summary>
    public int XP
    {
        get; set;
    }

    public CreatureSkills NotProficient
    {
        get; set;
    }

    public CreatureSkills HalfProficient
    {
        get; set;
    }

    public CreatureSkills Proficient
    {
        get; set;
    }

    public CreatureSkills Expertise
    {
        get; set;
    }

    public string ToolSkills
    {
        get; set;
    }

    public string Languages
    {
        get; set;
    }

    public override bool Equals(object? obj)
    {
        return (obj is CreatureBase creature && creature.Id == this.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
