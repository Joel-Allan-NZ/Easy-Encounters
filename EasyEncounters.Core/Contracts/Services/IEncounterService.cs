using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Models;

namespace EasyEncounters.Core.Contracts.Services;
public interface IEncounterService
{
    string DetermineDifficultyForParty(Encounter encounter, Party party);

    ActiveEncounterCreature CreateActiveEncounterCreature(Creature creature, bool maxHPRoll);
}
