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

    //EncounterDifficulty DetermineDifficultyForParty(Encounter encounter, double[] partyXPThreshold);

    IEnumerable<EncounterData> GenerateEncounterData(Party party, IEnumerable<Encounter> encounters);
    ///// <summary>
    ///// Calculate the party's XP thresholds; the poiints where an encounter's total XP could be expected
    ///// to change in relative difficulty. IE the XP value where you'd expect an encounter's difficulty to jump from Trivial to Easy, 
    ///// Easy to Medium etc.
    ///// </summary>
    ///// <param name="party"></param>
    ///// <returns></returns>
    //double[] GetPartyXPThreshold(Party party);

    void RemoveCreature(Encounter encounter, Creature creature);
}