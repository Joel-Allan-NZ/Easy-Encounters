using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Contracts.Services;

public interface IEncounterService
{
    /// <summary>
    /// Adds an NPC creature to the encounter. Not intended for adding party member NPCs or PCs; the encounter should remain reusable for other parties.
    /// </summary>
    /// <param name="encounter"></param>
    /// <param name="creature"></param>
    void AddCreature(Encounter encounter, Creature creature);

    /// <summary>
    /// Calculate the total experience value that represents the encounter's difficulty, including applying the official rule 
    /// multipliers for large numbers of monsters.
    /// </summary>
    /// <param name="encounter"></param>
    /// <returns></returns>
    double CalculateEncounterXP(Encounter encounter);

    EncounterDifficulty DetermineDifficultyForParty(Encounter encounter, Party party);

    void RemoveCreature(Encounter encounter, Creature creature);
}