using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Models;
public class ActiveEncounter : Encounter
{
    //going to build this with the assumption that only one encounter will ever actually be active. May be a bad idea if you're running multiple campaigns
    //and something unforeseen causes the session to end midcombat... but not worth coding around that edgecase.

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
    public Queue<ActiveEncounterCreature> CreatureTurns
    {
        get; set;
    }

    //todo: track turns so much more easily.
    public List<string> Log
    {
        get; set;
    }


    public ActiveEncounter()
    {
        //just to end annoying warnings the lazy way:
        ActiveCreatures = new List<ActiveEncounterCreature>();
        Log = new List<string>();
        CreatureTurns = new Queue<ActiveEncounterCreature>();


    }

    private void AddCreature(ActiveEncounterCreature creature, Dictionary<string, int> collisions)
    {
        if (collisions.ContainsKey(creature.EncounterName))
        {
            creature.EncounterName = creature.Name + " " + collisions[creature.Name];
            collisions[creature.Name]++;
        }
        else
            collisions[creature.Name] = 1;

        ActiveCreatures.Add(creature);
    }

    public ActiveEncounter(Encounter encounter, Party party)
    {
        this.ActiveCreatures = new List<ActiveEncounterCreature>();
        this.Name = encounter.Name;
        this.CreatureTurns = new Queue<ActiveEncounterCreature>();
        Log = new List<string>();

        Dictionary<string, int> nameCollisions = new();

        //this.Creatures = new List<Creature>(); //never really need to use/instantiate this one
        if (encounter.Creatures != null)
        {
            foreach (var creature in encounter.Creatures)
            {
                AddCreature(new ActiveEncounterCreature(creature), nameCollisions);
            }
            foreach (var member in party.Members)
            {
                AddCreature(new ActiveEncounterCreature(member), nameCollisions);
            }
        }
    }

    public ActiveEncounter(Encounter encounter, IEnumerable<ActiveEncounterCreature> creatures)
    {
        this.ActiveCreatures = new List<ActiveEncounterCreature>();
        this.Name = encounter.Name;
        this.CreatureTurns = new Queue<ActiveEncounterCreature>();
        Log = new List<string>();

        Dictionary<string, int> nameCollisions = new();

        foreach(var creature in creatures)
        {
            AddCreature(creature, nameCollisions);
        }
    }

}
