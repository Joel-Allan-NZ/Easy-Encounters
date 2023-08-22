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
                var tempCreature = new ActiveEncounterCreature(creature);

                tempCreature.Name ??= "Unnamed";

                if (nameCollisions.ContainsKey(tempCreature.Name))
                {
                    tempCreature.EncounterName = tempCreature.Name + " " + nameCollisions[tempCreature.Name];
                    nameCollisions[tempCreature.Name]++;
                }
                else
                    nameCollisions[tempCreature.Name] = 1;

                ActiveCreatures.Add(tempCreature);
            }
            foreach (var member in party.Members)
            {
                var tempCreature = new ActiveEncounterCreature(member);

                tempCreature.Name ??= "Unnamed";
                if (nameCollisions.ContainsKey(tempCreature.Name))
                {
                    tempCreature.EncounterName = tempCreature.Name + " " + nameCollisions[tempCreature.Name];
                    nameCollisions[tempCreature.Name]++;
                }
                else
                    nameCollisions[tempCreature.Name] = 1;

                ActiveCreatures.Add(tempCreature);
            }

            //roll initiative for NPCS? probably more sensible to kick that off at the saame time we establish initiative order. Let players roll first, then
            //hit the "get shit going" button.


        }
        //this.ActiveTurn = ActiveCreatures.First();//TODO: retool placeholder/default value? Current imp probably bad practice, but worry about this later. 
    }

}
