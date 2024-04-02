using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Models;

public class ActiveEncounterCreature : CreatureBase //: Creature
{
    public ActiveEncounterCreature(CreatureBase creature) : base(creature)
    {
        //Name = creature.Name;
        EncounterName = creature.Name ?? "Unnamed Creature";
        Dead = false;
        EncounterID = Guid.NewGuid();
        Reaction = true;
        Notes = "" + creature.Description;
        Concentrating = false;
        Initiative = -100; //placeholder value that indicates a creature hasn't rolled initiative

        CurrentLegendaryActions = creature.MaxLegendaryActions;
        CurrentLegendaryResistance = creature.MaxLegendaryResistance;
        ActiveConditions = Condition.None;

        ActiveSpellSlots = (int[])creature.SpellSlots.Clone();
        CreatureID = creature.Id;
        Id = Guid.NewGuid();

        Order = 0;

        //Creature = creature;
        //SpellSlots = new Dictionary<int, int>();

        //foreach (var kvp in creature.SpellSlots)
        //{
        //    SpellSlots.Add(kvp.Key, kvp.Value);
        //}
    }

    /// <summary>
    /// For easy ordering of initiative when loading activeencounter
    /// </summary>
    public int Order
    {
        get; set;
    }

    public Guid CreatureID
    {
        get; set;
    }

    public int[] ActiveSpellSlots
    {
        get; set;
    }

    //public Creature Creature
    //{
    //    get; set;
    //}

    public ActiveEncounterCreature()
    {
    }

    public List<ActiveAbility> ActiveAbilities
    {
        get; set;
    } = new();

    public Condition ActiveConditions
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

    public int CurrentHP
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

    /// <summary>
    /// Is the creature dead? If not then it shouldn't be in the turn order.
    /// </summary>
    public bool Dead
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
    /// A creature's maximum HP for the purposes of the active encounter; ie a rolled a value or default fixed value
    /// </summary>
    public int EncounterMaxHP
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
    /// The amount of temp HP a creature has. Damage is 'taken' from this before being added to total health damage taken.
    /// </summary>
    public int TempHP
    {
        get; set;
    }
}