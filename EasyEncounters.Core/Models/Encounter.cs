#nullable enable

namespace EasyEncounters.Core.Models;

public class Encounter : Persistable
{
    public Encounter(string name = "default", List<Creature>? creatures = null, string description = "", double adjustedEncounterXP = -1, Campaign? campaign = null)
    {
        Creatures = creatures ?? new List<Creature>();
        Name = name;
        Id = Guid.NewGuid();
        Description = description ?? "";
        AdjustedEncounterXP = adjustedEncounterXP;
        Campaign = campaign;
    }

    /// <summary>
    /// The effective total monster XP from the encounter, following DMG rules. Useful for measuring the difficulty
    /// of the encounter
    /// </summary>
    public double AdjustedEncounterXP
    {
        get; set;
    }

    /// <summary>
    /// The Campaign this encounter was designed for, if any
    /// </summary>
    public Campaign? Campaign
    {
        get; set;
    }

    /// <summary>
    /// Them varmints in the actual encounter
    /// </summary>
    public List<Creature> Creatures
    {
        get; set;
    }

    /// <summary>
    /// A short description for additional differentiation.
    /// </summary>
    public string Description
    {
        get; set;
    }

    /// <summary>
    /// The name of the encounter, for differentiation/preparing encounter groups ahead of time.
    /// </summary>
    public string Name
    {
        get; set;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is Encounter))
            return false;

        return ((Encounter)obj).Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}