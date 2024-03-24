using EasyEncounters.Core.Contracts;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Models;

public class Ability : IPersistable
{
    public Ability()
    {
        Id = Guid.NewGuid();
    }
    public Ability(string name = "New Ability", ResolutionType resolutionType = ResolutionType.Attack, int targetCount = 1,
        int targetSize = 0, int targetDistance = 5, ActionRangeType targetDistanceType = ActionRangeType.Self,
        TargetAreaType targetAreaType = TargetAreaType.Creatures,
        string effectDescription = "Add the ability's description here.", DamageType damageTypes = DamageType.None,
        CreatureAttributeType resolutionStat = CreatureAttributeType.None, SpellCastComponent spellCastComponents = SpellCastComponent.None,
        ActionSpeed actionSpeed = ActionSpeed.Action, string materialCost = "", SpellLevel spellLevel = SpellLevel.NotASpell,
        MagicSchool magicSchool = MagicSchool.None, int duration = 0, TimeDuration durationType = Enums.TimeDuration.Instantaneous,
        CreatureAttributeType saveType = CreatureAttributeType.None, bool concentration = false, string castTimeString = "")
    {
        Name = name;
        Resolution = resolutionType;
        TargetCount = targetCount;
        TargetAreaSize = targetSize;
        TargetDistance = targetDistance;
        TargetAreaType = targetAreaType;
        EffectDescription = effectDescription;
        CastingComponents = spellCastComponents;
        ResolutionStat = resolutionStat;
        DamageTypes = damageTypes;
        ActionSpeed = actionSpeed;
        MaterialCost = materialCost;
        SpellLevel = spellLevel;
        Id = Guid.NewGuid();
        TimeDuration = duration;
        TimeDurationType = durationType;
        TargetDistanceType = targetDistanceType;
        SaveType = saveType;
        MagicSchool = magicSchool;
        Concentration = concentration;
        CastTimeString = castTimeString;
    }

    /// <summary>
    /// The speed at which the ability can be used in the action economy - Reaction, Action, or Bonus Action.
    /// </summary>
    public ActionSpeed ActionSpeed
    {
        get; set;
    }

    /// <summary>
    /// The components involved in casting a spell - Verbal, Somatic, Material.
    /// </summary>
    public SpellCastComponent CastingComponents
    {
        get; set;
    }

    /// <summary>
    /// The length of time it takes to use an ability; for non-standard cast times.
    /// </summary>
    public string CastTimeString
    {
        get; set;
    }

    public bool Concentration
    {
        get; set;
    }

    /// <summary>
    /// The damage types the spell can do; for quick/easy damage set up during encounters.
    /// </summary>
    public DamageType DamageTypes
    {
        get; set;
    }

    /// <summary>
    /// String describing what effect the spell has. The spell's text.
    /// </summary>
    public string EffectDescription
    {
        get; set;
    }

    public Guid Id
    {
        get; set;
    }

    public MagicSchool MagicSchool
    {
        get; set;
    }

    /// <summary>
    /// A string representing the materials used to cast a spell with a material cost.
    /// </summary>
    public string MaterialCost
    {
        get; set;
    }

    public string Name
    {
        get; set;
    }

    /// <summary>
    /// The way the ability's effect is resolved. Save DC vs Attack roll etc.
    /// </summary>
    public ResolutionType Resolution
    {
        get; set;
    }

    /// <summary>
    /// The stat that the ability's resolution scales with. Ie +STR to hit with a regular weapon attack, or
    /// charisma for a warlock's spell DC etc.
    /// </summary>
    public CreatureAttributeType ResolutionStat
    {
        get; set;
    }

    public CreatureAttributeType SaveType
    {
        get; set;
    }

    /// <summary>
    /// The level of a spell, or an indicator that it's a cantrip or not a spell at all.
    /// </summary>
    public SpellLevel SpellLevel
    {
        get; set;
    }

    /// <summary>
    /// The size of the target area. 0 for abilities without an area.
    /// </summary>
    public int TargetAreaSize
    {
        get; set;
    }

    /// <summary>
    /// The type of targeting the ability has. Does it target creatures, or an cube, sphere etc.
    /// </summary>
    public TargetAreaType TargetAreaType
    {
        get; set;
    }

    /// <summary>
    /// The number of TargetAreaType this ability can target (usually 1, but exceptions like meteor swarm exist).
    /// </summary>
    public int TargetCount
    {
        get; set;
    }

    /// <summary>
    /// The maximum distance that the ability can be cast. Touch = 5 yards (melee range), Self = 0 yards (ie thunderclap that is centered on self).
    /// </summary>
    public int TargetDistance
    {
        get; set;
    }

    public ActionRangeType TargetDistanceType
    {
        get; set;
    }

    public int TimeDuration
    {
        get; set;
    }

    public TimeDuration TimeDurationType
    {
        get; set;
    }

    public override bool Equals(object obj)
    {
        if (obj != null && obj is Ability)
        {
            var abil = (Ability)obj;
            return abil.Id.Equals(this.Id);
        }
        return false;
    }

    public override int GetHashCode() => Id.GetHashCode();
}