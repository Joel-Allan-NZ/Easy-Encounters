#nullable enable

namespace EasyEncounters.Core.Models;

public class ActiveEncounter : Persistable
{
    //going to build this with the assumption that only one encounter will ever actually be active. May be a bad idea if you're running multiple campaigns
    //and something unforeseen causes the session to end midcombat... but not worth coding around that edgecase.

    public ActiveEncounter()
    {
        //just to end annoying warnings the lazy way:
        Name = "";
        Description = "";
        ActiveCreatures = new List<ActiveEncounterCreature>();
        Log = new List<string>();
        CreatureTurns = new List<ActiveEncounterCreature>();
    }

    public Party Party
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

    public ActiveEncounter(Encounter encounter, IEnumerable<ActiveEncounterCreature> creatures, Party party)
    {
        this.ActiveCreatures = new List<ActiveEncounterCreature>();
        this.Name = encounter.Name;
        this.CreatureTurns = new List<ActiveEncounterCreature>();
        this.Description = encounter.Description;
        Log = new List<string>();
        Id = Guid.NewGuid();

        Dictionary<string, int> nameCollisions = new();

        foreach (var creature in creatures)
        {
            AddCreature(creature, nameCollisions);
        }
        Party = party;
    }

    /// <summary>
    /// The creatures in this active encounter, their current state/s etc.
    /// </summary>
    public List<ActiveEncounterCreature> ActiveCreatures
    {
        get; set;
    }

    /// <summary>
    /// The creature whose turn it currently is.
    /// </summary>
    public ActiveEncounterCreature? ActiveTurn
    {
        get; set;
    }

    /// <summary>
    /// The creatures in the combat in turn order. Stays updated by en/de-queueing following each turn.
    /// </summary>
    public List<ActiveEncounterCreature> CreatureTurns //todo: does it make sense for this to be queue? It's ordered, but Queues are horrible for re-ordering which is meaningful
    {
        get; set;
    }

    //todo: track turns so much more easily.
    public List<string> Log
    {
        get; set;
    }

    private void AddCreature(ActiveEncounterCreature activeCreature, Dictionary<string, int> collisions)
    {
        if (collisions.ContainsKey(activeCreature.EncounterName))
        {
            activeCreature.EncounterName = activeCreature.Name + " " + collisions[activeCreature.Name];
            collisions[activeCreature.Name]++;
        }
        else
            collisions[activeCreature.Name] = 1;

        ActiveCreatures.Add(activeCreature);
    }

    /// <summary>
    /// A short description for additional differentiation.
    /// </summary>
    public string Description
    {
        get; set;
    }
}