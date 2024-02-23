using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Models;

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