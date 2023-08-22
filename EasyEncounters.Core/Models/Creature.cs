using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Models;
public class Creature : Persistable 
{
    /// <summary>
    /// The flat numeric bonus value to initiative rolls, including the bonus from dexterity.
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


    /// <summary>
    /// For initiative tie-breaking.
    /// </summary>
    public int DexBonus
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

    public Creature(string name = "New Creature", /*int dmgTaken =0, int tempHP =0,*/ int initBonus = 0,
        bool initAdv = false, bool dmControl = true, int dexBonus = 0, DamageType resistance = DamageType.None,
        DamageType immunity = DamageType.None, DamageType vuln = DamageType.None, string description = "",
        double levelCR = 0.0, string hyper = "https://www.dndbeyond.com/", int maxHP = 0, string maxHPString = "")
    {
        Name = name;
        //HealthDamageTaken = dmgTaken;
        //TempHP= tempHP;
        InitiativeBonus = initBonus;
        InitiativeAdvantage = initAdv;
        DMControl = dmControl;
        DexBonus = dexBonus;
        Immunity = immunity;
        Vulnerability = vuln;
        Resistance = resistance;
        Id = Guid.NewGuid();
        Description = description;
        LevelOrCR = levelCR;
        Hyperlink = hyper;
        MaxHP = maxHP;
        MaxHPString = maxHPString;
    }

    public void CopyFrom(Creature other)
    {
        Name = other.Name;
        InitiativeAdvantage = other.InitiativeAdvantage;
        DexBonus = other.DexBonus;
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
