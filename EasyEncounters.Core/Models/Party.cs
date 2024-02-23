#nullable enable

namespace EasyEncounters.Core.Models;

/// <summary>
/// For grouping a party of player characters together - smoother for adding them to active encounters etc.
/// </summary>
public class Party : Persistable
{
    public Party(Campaign? campaign = null, string name = "party", List<Creature>? members = null)
    {
        Campaign = campaign;
        Name = name;
        Members = members ?? new List<Creature>();
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// The campaign in which this party belongs... Which way around the party vs campaign inheritance goes is potentially a thorny subject,
    /// but this is how I choose to handle it for now.
    /// </summary>
    public Campaign? Campaign
    {
        get; set;
    }

    /// <summary>
    /// The members of the party.
    /// </summary>
    public List<Creature> Members
    {
        get; set;
    }

    /// <summary>
    /// A name for the party... probably not really required, but there's a world with more than one party perhaps?
    /// </summary>
    public string Name
    {
        get; set;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is Party))
            return false;

        return ((Party)obj).Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}