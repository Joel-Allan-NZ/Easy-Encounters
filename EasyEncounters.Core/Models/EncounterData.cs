using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Models;

/// <summary>
/// Data about a specific Encounter, and the Party expected to run it. Mostly used as
/// a convenient container for the encounter, party, and expected difficulty
/// </summary>
public class EncounterData
{
    public EncounterData(Encounter encounter, Party party, EncounterDifficulty difficultyDescription)
    {
        Party = party;
        Encounter = encounter;
        DifficultyDescription = difficultyDescription;//encounterService.DetermineDifficultyForParty(encounter, party);
    }

    public EncounterDifficulty DifficultyDescription
    {
        get; set;
    }

    public Encounter Encounter
    {
        get; set;
    }

    public Party Party
    {
        get; set;
    }
}