using System.Drawing;
using EasyEncounters.Core.Models.Enums;

#nullable enable

namespace EasyEncounters.Core.Models;

public class Creature : Persistable
{
    /// <summary>
    /// Creates a creature based on a shallow copy of the parameter creature.
    /// </summary>
    /// <param name="other"></param>
    public Creature(Creature other)
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

        SpellSlots = new Dictionary<int, int>(other.SpellSlots);
        Abilities = new List<Ability>(other.Abilities);

    }


    public Creature(string name = "New Creature", int initBonus = 0,
            bool initAdv = false, bool dmControl = true, DamageType resistance = DamageType.None,
            DamageType immunity = DamageType.None, DamageType vuln = DamageType.None, string description = "",
            double levelCR = 0.0, string hyper = "https://www.dndbeyond.com/", int maxHP = 0, string maxHPString = "", int aC = 10,
            int legActions = 0, int legRes = 0, string attackDescription = "", Condition conditionImmunities = Condition.None,
            int strength = 10, int strengthSave = 0, int dexterity = 10, int dexteritySave = 0, int constitution = 10, int constitutionSave = 0,
            int intelligence = 10, int intelligenceSave = 0, int wisdom = 10, int wisdomSave = 0, int charisma = 10, int charismaSave = 0,
            Dictionary<int, int>? spellSlots = null, int proficiencyBonus = 0, CreatureAttributeType spellStat = CreatureAttributeType.None,
            string features = "", string movement = "", string senses = "", CreatureType creatureType = CreatureType.Aberration, string creatureSubtype = "",
            CreatureAlignment creatureAlignment = CreatureAlignment.Undefined, CreatureSizeClass size = CreatureSizeClass.Medium)
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

        SpellStat = spellStat;
        Features = features;
        Movement = movement;

        Size = size;
        CreatureType = creatureType;
        Senses = senses;
        CreatureSubtype = creatureSubtype;
        Alignment = creatureAlignment;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Creature()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    
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

    public Dictionary<int, int> SpellSlots
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

    //public void CopyFrom(Creature other)
    //{
    //    Abilities.Clear();
    //    foreach (Ability ability in other.Abilities)
    //    {
    //        //copy spells as is, but clone other abilities
    //        if (ability.SpellLevel != SpellLevel.NotASpell)
    //            Abilities.Add(ability);
    //        else
    //        {
    //            //TODO: add ability copying for non-spells-
    //            var newAbility = new Ability();
    //        }
    //    }

    //    Name = other.Name;
    //    InitiativeAdvantage = other.InitiativeAdvantage;
    //    InitiativeBonus = other.InitiativeBonus;
    //    DMControl = other.DMControl;
    //    Description = other.Description;
    //    LevelOrCR = other.LevelOrCR;
    //    Immunity = other.Immunity;
    //    Vulnerability = other.Vulnerability;
    //    Resistance = other.Resistance;
    //    Hyperlink = other.Hyperlink;
    //    MaxHP = other.MaxHP;
    //    MaxHPString = other.MaxHPString;
    //    AC = other.AC;
    //    MaxLegendaryResistance = other.MaxLegendaryResistance;
    //    MaxLegendaryActions = other.MaxLegendaryActions;
    //    ConditionImmunities = other.ConditionImmunities;
    //    AttackDescription = other.AttackDescription;
    //    Strength = other.Strength;
    //    StrengthSave = other.StrengthSave;
    //    Dexterity = other.Dexterity;
    //    DexteritySave = other.DexteritySave;
    //    Constitution = other.Constitution;
    //    ConstitutionSave = other.ConstitutionSave;
    //    Intelligence = other.Intelligence;
    //    IntelligenceSave = other.IntelligenceSave;
    //    Wisdom = other.Wisdom;
    //    WisdomSave = other.WisdomSave;
    //    Charisma = other.Charisma;
    //    CharismaSave = other.CharismaSave;
    //    Movement = other.Movement;
    //    ProficiencyBonus = other.ProficiencyBonus;

    //    SpellStat = other.SpellStat;
    //    Features = other.Features;

    //    Size = other.Size;
    //    CreatureType = other.CreatureType;
    //    Senses = other.Senses;
    //    CreatureSubtype = other.CreatureSubtype;
    //    Alignment = other.Alignment;

    //    SpellSlots.Clear();
    //    foreach (var kvp in other.SpellSlots)
    //    {
    //        SpellSlots.Add(kvp.Key, kvp.Value);
    //    }
    //}

    public override bool Equals(object? obj)
    {
        return (obj is Creature creature && creature.Id == Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}