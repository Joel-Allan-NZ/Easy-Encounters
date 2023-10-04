using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Models;

namespace EasyEncounters.Core.Contracts.Services;
public interface IEncounterService
{
    int[] GetPartyXPThreshold(Party party);

    string DetermineDifficultyForParty(Encounter encounter, Party party);

    string DetermineDifficultyForParty(Encounter encounter, int[] partyXPThreshold);

    ActiveEncounterCreature CreateActiveEncounterCreature(Creature creature, bool maxHPRoll);
    void AddCreature(Encounter encounter, Creature creature);
    void RemoveCreature(Encounter encounter, Creature creature);
    double CalculateEncounterXP(Encounter encounter);
}
