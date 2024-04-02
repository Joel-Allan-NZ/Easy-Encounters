#nullable enable

namespace EasyEncounters.Core.Models;

/// <summary>
/// Represents a DND encounter / fight / combat
/// </summary>
public class Encounter : Persistable
{
    public Encounter()
    {
        
    }
    public Encounter(string name = "default", List<Creature>? creatures = null, string description = "", 
        double adjustedEncounterXP = -1, Campaign? campaign = null, bool isCampaignOnlyEncounter = false, List<EncounterCreatures>? byCount = null, string plan = "")
    {
        Creatures = creatures ?? new List<Creature>();
        CreaturesByCount = byCount ?? new();

        if (CreaturesByCount.Count == 0)
        {
            Dictionary<Creature, int> critters = new();
            foreach (var creature in Creatures)
            {
                if (critters.ContainsKey(creature))
                {
                    critters[creature] += 1;
                }
                else
                {
                    critters[creature] = 1;
                }
            }
            foreach (var k in critters.Keys)
            {
                CreaturesByCount.Add(new EncounterCreatures(k, critters[k]));
            }
        }
        CreatureCount = CreaturesByCount.Sum(x => x.Count);
        Name = name;
        Id = Guid.NewGuid();
        Description = description ?? "";
        AdjustedEncounterXP = adjustedEncounterXP;
        Campaign = campaign;
        IsCampaignOnlyEncounter = isCampaignOnlyEncounter;
        Plan = plan;
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
    /// Whether or not the encounter is explicitly designed for a specific campaign.
    /// </summary>
    public bool IsCampaignOnlyEncounter
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

    public List<EncounterCreatures> CreaturesByCount
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
        return obj != null && obj is Encounter encounter && encounter.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public int CreatureCount
    {
        get; set;
    }

    public string Plan
    {
        get; set;
    }
}