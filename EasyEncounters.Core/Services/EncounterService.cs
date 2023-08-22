﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;

namespace EasyEncounters.Core.Services;
public class EncounterService : IEncounterService
{
    readonly IPartyXPService _partyXPService;
    readonly IDiceService _diceService;
    public EncounterService(IPartyXPService partyXPService, IDiceService diceService)
    {
        _diceService = diceService;
        _partyXPService = partyXPService;

    }

    public string DetermineDifficultyForParty(Encounter encounter, Party party)
    {
        if (encounter.Creatures == null || encounter.Creatures.Count == 0)
        {
            return "N/A";
        }
        else
        {
            var partyXPThreshold = _partyXPService.FindPartyXPThreshold(party);
            var monsterXPTotal = 0;
            foreach (var creature in encounter.Creatures)
            {
                monsterXPTotal += MonsterXPFromCR(creature.LevelOrCR);
            }
            double adjustedMonsterXP = monsterXPTotal * EncounterSizeMulitiplier(EffectiveMonsterCount(encounter));

            if (adjustedMonsterXP < partyXPThreshold[0])
                return "Trivial";

            if (adjustedMonsterXP < partyXPThreshold[1])
                return "Easy";

            if (adjustedMonsterXP < partyXPThreshold[2])
                return "Medium";

            if (adjustedMonsterXP < partyXPThreshold[3])
                return "Hard";

            if (adjustedMonsterXP < partyXPThreshold[3] * 1.5)
                return "Deadly";

            return "Very Deadly";
        }
    }

    private int MonsterXPFromCR(double CR)
    {
        switch (CR)
        {
            case 0:
                return 10;
            case 1:
                return 200;
            case 2:
                return 450;
            case 3:
                return 700;
            case 4:
                return 1100;
            case 5:
                return 1800;
            case 6:
                return 2300;
            case 7:
                return 2900;
            case 8:
                return 3900;
            case 9:
                return 5000;
            case 10:
                return 5900;
            case 11:
                return 7200;
            case 12:
                return 8400;
            case 13:
                return 10000;
            case 14:
                return 11500;
            case 15:
                return 13000;
            case 16:
                return 15000;
            case 17:
                return 18000;
            case 18:
                return 20000;
            case 19:
                return 22000;
            case 20:
                return 22000;
            case 21:
                return 33000;
            case 22:
                return 41000;
            case 23:
                return 50000;
            case 24:
                return 62000;
            case 25:
                return 75000;
            case 26:
                return 90000;
            case 27:
                return 105000;
            case 28:
                return 120000;
            case 29:
                return 135000;
            case 30:
                return 155000;

            default:
                {
                    if (CR > .124 && CR < .126)
                        return 25;
                    else if (CR > .24 && CR < .26)
                        return 50;
                    else if (CR > .49 && CR < .51)
                        return 100;
                    else return 1000000;
                }
        }
    }

    /// <summary>
    /// When calculating the number of enemies in the encounter, 
    /// you should only include those who aren't significantly below
    /// the average CR.
    /// </summary>
    /// <returns></returns>
    private int EffectiveMonsterCount(Encounter encounter)
    {
        var averageCR = encounter.Creatures.Average(x => x.LevelOrCR);
        int creatureCount = encounter.Creatures.Count;
        if (averageCR != 0)
        {
            creatureCount = 0;
            foreach (var creature in encounter.Creatures)
            {
                if (creature.LevelOrCR > averageCR / 3)
                {
                    creatureCount++;
                }
            }
        }
        return creatureCount;
    }

    private double EncounterSizeMulitiplier(int encounterSize)
    {
        switch (encounterSize)
        {
            case 1:
                return 1;
            case 2:
                return 1.5;
            case < 7:
                return 2;
            case < 11:
                return 2.5;
            case < 15:
                return 3;
            default:
                return 4;
        }
    }

    public ActiveEncounterCreature CreateActiveEncounterCreature(Creature creature, bool maxHPRoll)
    {
        ActiveEncounterCreature result = new ActiveEncounterCreature(creature);
        if (maxHPRoll)
        {
            result.EncounterMaxHP = _diceService.Roll(creature.MaxHPString);
            result.CurrentHP = result.EncounterMaxHP;
        }
        return result;
    }
}
