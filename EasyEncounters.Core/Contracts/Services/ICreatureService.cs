using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Contracts.Services;
public interface ICreatureService
{
    int GetAttributeTypeValue(Creature creature, CreatureAttributeType creatureAttributeType);

    /// <summary>
    /// Get the bonus value for a specified attribute on a specified creature. 
    /// </summary>
    /// <param name="creature"></param>
    /// <param name="creatureAttributeType"></param>
    /// <returns>Bonus value of an attribute, ie +2</returns>
    int GetAttributeBonusValue(Creature creature, CreatureAttributeType creatureAttributeType);

    void CopyTo(Creature creatureTarget, Creature creatureSource);
}
