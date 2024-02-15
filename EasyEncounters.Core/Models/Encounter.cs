using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable enable

namespace EasyEncounters.Core.Models;
public class Encounter : Persistable
{
    /// <summary>
    /// The name of the encounter, for differentiation/preparing encounter groups ahead of time.
    /// </summary>
    public string Name
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
    /// The effective total monster XP from the encounter, following DMG rules. Useful for measuring the difficulty
    /// of the encounter
    /// </summary>
    public double AdjustedEncounterXP
    {
        get; set;
    }

    public Encounter(string name = "default", List<Creature>? creatures = null, string description = "", double adjustedEncounterXP = -1)
    {
        Creatures = creatures ?? new List<Creature>();
        Name = name;
        Id = Guid.NewGuid();
        Description = description ?? "";
        AdjustedEncounterXP = adjustedEncounterXP;
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