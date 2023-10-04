﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Contracts.Services;

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

    public string DifficultyDescription
    {
        get; set;
    }

    public EncounterData(Encounter encounter, Party party, string difficultyDescription)
    {
        Party = party;
        Encounter = encounter;
        DifficultyDescription = difficultyDescription;//encounterService.DetermineDifficultyForParty(encounter, party);
    }
}
