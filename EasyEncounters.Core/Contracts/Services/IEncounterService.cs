using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Contracts.Services;

public interface IEncounterService
{
    void AddCreature(Encounter encounter, Creature creature);

    double CalculateEncounterXP(Encounter encounter);

    EncounterDifficulty DetermineDifficultyForParty(Encounter encounter, Party party);

    EncounterDifficulty DetermineDifficultyForParty(Encounter encounter, double[] partyXPThreshold);

    IEnumerable<EncounterData> GenerateEncounterData(Party party, IEnumerable<Encounter> encounters);
    double[] GetPartyXPThreshold(Party party);

    void RemoveCreature(Encounter encounter, Creature creature);
}