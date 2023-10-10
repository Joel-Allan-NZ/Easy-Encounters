using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Services;
public class EncounterService : IEncounterService
{
    readonly IPartyXPService _partyXPService;
    readonly IDiceService _diceService;
    readonly ICreatureService _creatureService;
    readonly IAbilityService _abilityService;
    public EncounterService(IPartyXPService partyXPService, IDiceService diceService, IAbilityService abilityService, ICreatureService creatureService)
    {
        _diceService = diceService;
        _partyXPService = partyXPService;
        _abilityService = abilityService;
        _creatureService = creatureService;

    }

    public void AddCreature(Encounter encounter, Creature creature)
    {
        encounter.Creatures.Add(creature);
        encounter.AdjustedEncounterXP = CalculateEncounterXP(encounter);
    }

    public void RemoveCreature(Encounter encounter, Creature creature)
    {
        encounter.Creatures.Remove(creature);
        encounter.AdjustedEncounterXP = CalculateEncounterXP(encounter);
    }


    public int[] GetPartyXPThreshold(Party party) => _partyXPService.FindPartyXPThreshold(party);

    public EncounterDifficulty DetermineDifficultyForParty(Encounter encounter, int[] partyXPThreshold)
    {
        if (encounter.Creatures == null || encounter.Creatures.Count == 0)
            return EncounterDifficulty.None; //"N/A";

        if (encounter.AdjustedEncounterXP == -1)
            encounter.AdjustedEncounterXP = CalculateEncounterXP(encounter);


        if (encounter.AdjustedEncounterXP < partyXPThreshold[0])
            return EncounterDifficulty.Trivial; //"Trivial";

        if (encounter.AdjustedEncounterXP < partyXPThreshold[1])
            return EncounterDifficulty.Easy; //"Easy";

        if (encounter.AdjustedEncounterXP < partyXPThreshold[2])
            return EncounterDifficulty.Medium;//"Medium";

        if (encounter.AdjustedEncounterXP < partyXPThreshold[3])
            return EncounterDifficulty.Hard;//"Hard";

        if (encounter.AdjustedEncounterXP < partyXPThreshold[3] * 1.5)
            return EncounterDifficulty.Deadly;//"Deadly";

        return EncounterDifficulty.VeryDeadly;//"Very Deadly";
    }

    public double CalculateEncounterXP(Encounter encounter)
    {
        if (encounter.Creatures.Count == 0)
            return -1;

        var monsterXPTotal = 0;
        foreach (var creature in encounter.Creatures)
        {
            monsterXPTotal += MonsterXPFromCR(creature.LevelOrCR);
        }
        return monsterXPTotal * EncounterSizeMulitiplier(EffectiveMonsterCount(encounter));

    }

    public EncounterDifficulty DetermineDifficultyForParty(Encounter encounter, Party party)
    {
        return DetermineDifficultyForParty(encounter, GetPartyXPThreshold(party));
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
        ActiveEncounterCreature result = new ActiveEncounterCreature(creature);// new ActiveEncounterCreature(creature);
        _creatureService.CopyTo(result, creature);
        if (maxHPRoll && creature.DMControl)
        {
            result.EncounterMaxHP = _diceService.Roll(creature.MaxHPString);
            result.CurrentHP = result.EncounterMaxHP;
        }
        else
        {
            result.EncounterMaxHP = result.MaxHP;
            result.CurrentHP = result.MaxHP; //todo: long term we need to improve this for parties specifically - should be able to track friendly NPC health between
            //encounters. Gets messy with rests, but that's a future issue
        }

        foreach (var ability in creature.Abilities)
        {
            if(ability.SpellLevel != Models.Enums.SpellLevel.NotASpell)
            {
                result.ActiveAbilities.Add(_abilityService.CreateActiveAbility(result, ability, _creatureService.GetAttributeBonusValue(result, creature.SpellStat)));
            }
            else
                result.ActiveAbilities.Add(_abilityService.CreateActiveAbility(result, ability, _creatureService.GetAttributeBonusValue(result, ability.ResolutionStat)));
        }

        return result;
    }
}
