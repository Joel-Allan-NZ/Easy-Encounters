using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEncounters.Core.Models;
public class EncounterCreatures : Persistable
{
    public EncounterCreatures(Creature creature, int count)
    {
        Count = count;
        Creature = creature;
        Id = Guid.NewGuid();
    }

    public EncounterCreatures()
    {
        
    }


    public Creature Creature
    {
        get; set;
    }

    public int Count
    {
        get; set;
    }
}
