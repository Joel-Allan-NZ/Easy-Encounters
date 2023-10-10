using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Models;
public class EncounterData
{
    public Encounter Encounter
    {
        get; set;
    }

    public Party Party
    {
        get; set;
    }

    public EncounterDifficulty DifficultyDescription
    {
        get; set;
    }

    public EncounterData(Encounter encounter, Party party, EncounterDifficulty difficultyDescription)
    {
        Party = party;
        Encounter = encounter;
        DifficultyDescription = difficultyDescription;//encounterService.DetermineDifficultyForParty(encounter, party);
    }
}
