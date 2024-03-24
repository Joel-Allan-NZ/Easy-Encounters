using EasyEncounters.Core.Contracts.Enums;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Services;

public class ActiveEncounterService : IActiveEncounterService
{
    private readonly IAbilityService _abilityService;
    private readonly ICreatureService _creatureService;
    private readonly IDataService _dataService;
    private readonly IDiceService _diceService;
    private readonly IEncounterService _encounterService;
    private readonly ILogService _logService;
    private readonly IModelOptionsService _modelOptionsService;

    public ActiveEncounterService(IDiceService diceService, IDataService dataService, IEncounterService encounterService, ICreatureService creatureService, ILogService logService,
        IAbilityService abilityService, IModelOptionsService modelOptionsService)
    {
        _encounterService = encounterService;
        _dataService = dataService;
        _creatureService = creatureService;
        _logService = logService;
        _diceService = diceService;
        _abilityService = abilityService;
        _modelOptionsService = modelOptionsService;

    }

    public void AddCreatureToInProgressEncounter(ActiveEncounter inProgress, Creature creatureToAdd)
    {
        var activeCreatureToAdd = CreateActiveEncounterCreature(creatureToAdd, _modelOptionsService.RollHP);

        var nameSuffixDigit = inProgress.ActiveCreatures.Count(x => x.Name == creatureToAdd.Name);

        activeCreatureToAdd.EncounterName = nameSuffixDigit == 0 ? creatureToAdd.Name : $"{creatureToAdd.Name} {nameSuffixDigit}";

        inProgress.ActiveCreatures.Add(activeCreatureToAdd);
        inProgress.CreatureTurns.Add(activeCreatureToAdd);

        RollInitiative(activeCreatureToAdd);
    }

    public async Task<ActiveEncounter> CreateActiveEncounterAsync(Encounter encounter, Party party)
    {
        var activeCreatures = CreateCreaturesForActiveEncounter(encounter, party);

        var active = new ActiveEncounter(encounter, activeCreatures);
        active.Description ??= "Active";
        await _dataService.SaveAddAsync(active);

        return active;
    }

    public async Task<string> DealDamageAsync(ActiveEncounter activeEncounter, DamageInstance damageInstance)
    {
        return await DealDamageAsync(activeEncounter, damageInstance.Source, damageInstance.Target, damageInstance.DamageType, GetEffectiveDamage(damageInstance.BaseDamageValue, damageInstance.DamageVolume));
    }

    public async Task<string> EndCurrentTurnAsync(ActiveEncounter activeEncounter)
    {
        activeEncounter.ActiveTurn = FindNextCreatureTurn(activeEncounter);

        //clean out the dead
        activeEncounter.CreatureTurns = activeEncounter.CreatureTurns.Where(x => !x.Dead).ToList();

        //new turn maintenance
        activeEncounter.ActiveTurn.Reaction = true;
        activeEncounter.ActiveTurn.CurrentLegendaryActions = activeEncounter.ActiveTurn.MaxLegendaryActions;

        await _dataService.SaveAddAsync(activeEncounter);
        _logService.LogTurnEnd();
        return _logService.LogTurnStart(activeEncounter.ActiveTurn);
    }

    public async Task EndEncounterAsync(ActiveEncounter activeEncounter)
    {
        await _dataService.WriteLogAsync(activeEncounter.Log);
        //await _dataService.ClearActiveEncounterAsync();
        await _logService.EndEncounterLog();
    }

    public DamageVolume GetDamageVolumeSuggestion(ActiveEncounterCreature target, DamageType damageType)
    {
        var suggestedDamageVolume = DamageVolume.Normal;

        if (target.Vulnerability.HasFlag(damageType))
        {
            suggestedDamageVolume = DamageVolume.Double;
        }
        else if (target.Resistance.HasFlag(damageType))
        {
            suggestedDamageVolume = DamageVolume.Half;
        }
        else if (target.Immunity.HasFlag(damageType))
        {
            suggestedDamageVolume = DamageVolume.None;
        }

        return suggestedDamageVolume;
    }

    public void OrderCreatureTurns(ActiveEncounter activeEncounter, IEnumerable<ActiveEncounterCreature> creatures)
    {
        activeEncounter.CreatureTurns = creatures.ToList();
    }

    public async Task<IEnumerable<ActiveEncounterCreature>> RollInitiative(ActiveEncounter activeEncounter)
    {
        foreach (var unrolled in activeEncounter.ActiveCreatures.Where(x => x.Initiative == -100 && x.DMControl))
        {
            RollInitiative(unrolled);
        }

        OrderInitiative(activeEncounter);

        activeEncounter.ActiveTurn ??= activeEncounter.CreatureTurns.First();

        await _dataService.SaveAddAsync(activeEncounter);
        _logService.StartEncounterLog(activeEncounter.ActiveTurn);

        return activeEncounter.CreatureTurns;
    }

    private static int GetEffectiveDamage(int damage, DamageVolume multiplierEnum)
    {
        var result = damage;
        if (multiplierEnum == DamageVolume.Double)
            result *= 2;
        else if (multiplierEnum == DamageVolume.Half)
            result /= 2;
        else if (multiplierEnum == DamageVolume.None)
            result = 0;
        else if (multiplierEnum == DamageVolume.Quarter)
        {
            //correct calculation for this is to halve it twice, rounding down each time.
            result /= 2;
            result /= 2;
        }
        return result;
    }

    private void AddActiveAbilities(Creature creature, ActiveEncounterCreature result)
    {
        foreach (var ability in creature.Abilities)
        {
            var resolutionStat = ability.SpellLevel > 0 ? creature.SpellStat : ability.ResolutionStat;

            result.ActiveAbilities.Add(_abilityService.CreateActiveAbility(result, ability, _creatureService.GetAttributeBonusValue(result, resolutionStat)));
        }
    }

    private ActiveEncounterCreature CreateActiveEncounterCreature(Creature creature, bool rollHP)
    {
        var result = new ActiveEncounterCreature(creature);

        if (rollHP && creature.DMControl)
        {
            result.EncounterMaxHP = string.IsNullOrEmpty(creature.MaxHPString) ? creature.MaxHP : _diceService.Roll(creature.MaxHPString);
        }
        else
        {
            result.EncounterMaxHP = result.MaxHP;
            //todo: long term we need to improve this for parties specifically - should be able to track friendly NPC health between
            //encounters. Gets messy with rests, but that's a future issue
        }

        result.CurrentHP = result.EncounterMaxHP;

        AddActiveAbilities(creature, result);

        return result;
    }

    private IEnumerable<ActiveEncounterCreature> CreateCreaturesForActiveEncounter(Encounter encounter, Party party)
    {
        var activeCreatures = encounter.Creatures.Select(x => CreateActiveEncounterCreature(x, _modelOptionsService.RollHP));

        return activeCreatures.Concat(party.Members.Select(x => CreateActiveEncounterCreature(x, false))); //todo: investigate if this deferred execution here is ever a problem
    }

    private async Task<string> DealDamageAsync(ActiveEncounter activeEncounter, ActiveEncounterCreature sourceCreature, ActiveEncounterCreature targetCreature, DamageType damageType, int damageValue)
    {
        if (damageType == DamageType.Healing)
        {
            targetCreature.CurrentHP = Math.Min(targetCreature.CurrentHP + damageValue, targetCreature.MaxHP);
        }
        else
        {
            targetCreature.CurrentHP -= damageValue;

            targetCreature.Dead = (targetCreature.MaxHP != 0 && targetCreature.CurrentHP <= 0); //non-pc character dead'd
        }

        return await _logService.LogDamage(damageType, sourceCreature, targetCreature, damageValue);
    }

    private ActiveEncounterCreature FindNextCreatureTurn(ActiveEncounter activeEncounter)
    {
        //check after first
        var nextIndex = activeEncounter.CreatureTurns.IndexOf(activeEncounter.ActiveTurn) + 1;
        if (nextIndex == 0)
            throw new Exception("No living creatures left to take a turn.");

        var cycle = activeEncounter.CreatureTurns.Skip(nextIndex).Concat(activeEncounter.CreatureTurns.Take(nextIndex));

        return cycle.FirstOrDefault(x => !x.Dead);
    }

    private void OrderInitiative(ActiveEncounter activeEncounter)
    {
        var sortSource = activeEncounter.CreatureTurns.Count > 0 ? activeEncounter.CreatureTurns : activeEncounter.ActiveCreatures;

        //clean(ish) tiebreaking by roll, then dex bonus, then finally if PC or NPC (advantage to PC)
        activeEncounter.CreatureTurns = sortSource.OrderByDescending(x => x.Initiative + ((double)_creatureService.GetAttributeBonusValue(x, CreatureAttributeType.Dexterity) / 100))
                                                  .ThenByDescending(y => y.DMControl)
                                                  .ToList();
    }

    private void RollInitiative(ActiveEncounterCreature activeEncounterCreature)
    {
        var advantageModifier = activeEncounterCreature.InitiativeAdvantage ? DiceRollModifier.Advantage : DiceRollModifier.None;

        activeEncounterCreature.Initiative = _diceService.Roll(20, 1, advantageModifier)
            + activeEncounterCreature.InitiativeBonus
            + _creatureService.GetAttributeBonusValue(activeEncounterCreature, CreatureAttributeType.Dexterity);
    }
}