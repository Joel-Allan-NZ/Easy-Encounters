using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Contracts.Services;

public interface IEncounterService
{
    void AddCreature(Encounter encounter, Creature creature);

    double CalculateEncounterXP(Encounter encounter);

    ActiveEncounterCreature CreateActiveEncounterCreature(Creature creature, bool maxHPRoll);

    EncounterDifficulty DetermineDifficultyForParty(Encounter encounter, Party party);

    EncounterDifficulty DetermineDifficultyForParty(Encounter encounter, int[] partyXPThreshold);

    int[] GetPartyXPThreshold(Party party);

    void RemoveCreature(Encounter encounter, Creature creature);
}