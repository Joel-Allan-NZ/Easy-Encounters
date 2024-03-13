using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Services;

public class EncounterService : IEncounterService
{
    private readonly IPartyXPService _partyXPService;

    public EncounterService(IPartyXPService partyXPService)
    {
        _partyXPService = partyXPService;
    }

    public void AddCreature(Encounter encounter, Creature creature)
    {
        encounter.Creatures.Add(creature);
        encounter.AdjustedEncounterXP = CalculateEncounterXP(encounter);
    }

    public double CalculateEncounterXP(Encounter encounter)
    {
        if (encounter.Creatures.Count == 0)
        {
            return -1;
        }

        var monsterXPTotal = 0;
        foreach (var creature in encounter.Creatures)
        {
            monsterXPTotal += MonsterXPFromCR(creature.LevelOrCR);
        }
        encounter.AdjustedEncounterXP = monsterXPTotal * EncounterSizeMulitiplier(EffectiveMonsterCount(encounter));
        return encounter.AdjustedEncounterXP;
    }

    public EncounterDifficulty DetermineDifficultyForParty(Encounter encounter, double[] partyXPThreshold)
    {
        if (encounter.Creatures == null || encounter.Creatures.Count == 0)
        {
            return EncounterDifficulty.None;
        }

        if (encounter.AdjustedEncounterXP == -1)
        {
            CalculateEncounterXP(encounter);
        }

        var thresholdCount = partyXPThreshold.Count(x => encounter.AdjustedEncounterXP > x);

        return (EncounterDifficulty)thresholdCount + 1;
    }

    public EncounterDifficulty DetermineDifficultyForParty(Encounter encounter, Party party)
    {
        return DetermineDifficultyForParty(encounter, GetPartyXPThreshold(party));
    }

    public double[] GetPartyXPThreshold(Party party)
    {
        _partyXPService.CalculatePartyXPThresholds(party); //TODO:deprecate
        return party.PartyXPThresholds;
    }

    public void RemoveCreature(Encounter encounter, Creature creature)
    {
        encounter.Creatures.Remove(creature);
        encounter.AdjustedEncounterXP = CalculateEncounterXP(encounter);
    }

    /// <summary>
    /// When calculating the number of enemies in the encounter,
    /// you should only include those who aren't significantly below
    /// the average CR.
    /// </summary>
    /// <returns></returns>
    private static int EffectiveMonsterCount(Encounter encounter)
    {
        var averageCR = encounter.Creatures.Average(x => x.LevelOrCR);
        return encounter.Creatures.Count(x => x.LevelOrCR > (averageCR / 3));
    }

    private static double EncounterSizeMulitiplier(int encounterSize)
    {
        return encounterSize switch
        {
            1 => 1,
            2 => 1.5,
            < 7 => 2,
            < 11 => 2.5,
            < 15 => 3,
            _ => 4,
        };
    }

    private static int MonsterXPFromCR(double CR)
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
                    {
                        return 25;
                    }
                    else if (CR > .24 && CR < .26)
                    {
                        return 50;
                    }
                    else if (CR > .49 && CR < .51)
                    {
                        return 100;
                    }
                    else
                    {
                        return 1000000;
                    }
                }
        }
    }
}