using System.Text;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Services;

public class ActiveEncounterService : IActiveEncounterService
{
    private readonly ICreatureService _creatureService;
    private readonly IDataService _dataService;
    private readonly IEncounterService _encounterService;
    private readonly ILogService _logService;
    private readonly IRandomService _randomService;

    public ActiveEncounterService(IRandomService randomService, IDataService dataService, IEncounterService encounterService, ICreatureService creatureService, ILogService logService)
    {
        _encounterService = encounterService;
        _randomService = randomService;
        _dataService = dataService;
        _creatureService = creatureService;
        _logService = logService;
    }

    public bool RollHP
    {
        get; set;
    } = false; //defaults to false (ie Fixed HP is the default)

    public void AddCreatureToInProgressEncounter(ActiveEncounter inProgress, Creature creatureToAdd)
    {
        var activeCreatureToAdd = _encounterService.CreateActiveEncounterCreature(creatureToAdd, RollHP);
        var collisionCount = inProgress.ActiveCreatures.Count(x => x.Name == creatureToAdd.Name);

        activeCreatureToAdd.EncounterName = $"{creatureToAdd.Name} {collisionCount}";

        inProgress.ActiveCreatures.Add(activeCreatureToAdd);
        RollInitiative(activeCreatureToAdd);

        inProgress.CreatureTurns.Enqueue(activeCreatureToAdd);
    }

    public async Task<ActiveEncounter> CreateActiveEncounterAsync(Encounter encounter, Party party)
    {
        var activeCreatures = CreateCreaturesForActiveEncounter(encounter, party);

        var active = new ActiveEncounter(encounter, activeCreatures);
        await _dataService.SaveAddAsync(active);

        return active;
    }

    public async Task<string> DealDamageAsync(ActiveEncounter activeEncounter, DamageInstance damageInstance)
    {
        return await DealDamageAsync(activeEncounter, damageInstance.Source, damageInstance.Target, damageInstance.DamageType, GetEffectiveDamage(damageInstance.BaseDamageValue, damageInstance.DamageVolume));
    }

    public async Task<string> EndCurrentTurnAsync(ActiveEncounter activeEncounter)
    {
        if (activeEncounter.ActiveTurn != null && !activeEncounter.ActiveTurn.Dead)
            activeEncounter.CreatureTurns.Enqueue(activeEncounter.CreatureTurns.Dequeue());
        //clear active turn, then ensure it gets cycled to the next living creature, clearing dead ones from the initiative order.
        activeEncounter.ActiveTurn = null;
        while (activeEncounter.CreatureTurns.Peek().Dead)
        {
            activeEncounter.CreatureTurns.Dequeue();
        }
        activeEncounter.ActiveTurn = activeEncounter.CreatureTurns.Peek();
        activeEncounter.CreatureTurns.Enqueue(activeEncounter.CreatureTurns.Dequeue());
        //clear out all remaining dead creatures.
        while (activeEncounter.CreatureTurns.Peek() != activeEncounter.ActiveTurn)
        {
            if (activeEncounter.CreatureTurns.Peek().Dead)
                activeEncounter.CreatureTurns.Dequeue();
            else
                activeEncounter.CreatureTurns.Enqueue(activeEncounter.CreatureTurns.Dequeue());
        }

        activeEncounter.ActiveTurn.Reaction = true;
        activeEncounter.ActiveTurn.CurrentLegendaryActions = activeEncounter.ActiveTurn.MaxLegendaryActions;

        await _dataService.SaveAddAsync(activeEncounter);
        _logService.LogTurnEnd();
        return _logService.LogTurnStart(activeEncounter.ActiveTurn);
    }

    public async Task EndEncounterAsync(ActiveEncounter activeEncounter)
    {
        await _dataService.WriteLog(activeEncounter.Log);
        await _dataService.ClearActiveEncounterAsync();
        await _logService.EndEncounterLog();
    }

    public DamageVolume GetDamageVolumeSuggestion(ActiveEncounterCreature target, DamageType damageType)
    {
        DamageVolume suggestedDamageVolume = DamageVolume.Normal;
        if (target.Vulnerability.HasFlag(damageType))
            suggestedDamageVolume = DamageVolume.Double;
        else if (target.Resistance.HasFlag(damageType))
            suggestedDamageVolume = DamageVolume.Half;
        else if (target.Immunity.HasFlag(damageType))
            suggestedDamageVolume = DamageVolume.None;

        return suggestedDamageVolume;
    }

    public void ReorderInitiative(ActiveEncounter activeEncounter, IEnumerable<ActiveEncounterCreature> creatures)
    {
        activeEncounter.CreatureTurns.Clear();
        foreach (var creature in creatures)
            activeEncounter.CreatureTurns.Enqueue(creature);

        while (activeEncounter.ActiveTurn != activeEncounter.CreatureTurns.Peek())
        {
            activeEncounter.CreatureTurns.Enqueue(activeEncounter.CreatureTurns.Dequeue());
        }
    }

    public async Task<IEnumerable<ActiveEncounterCreature>> UpdateInitiativeOrder(ActiveEncounter activeEncounter)
    {
        if (activeEncounter.ActiveTurn == null)
        {
            foreach (var activeCreature in activeEncounter.ActiveCreatures)
            {
                if (activeCreature.Initiative == -100 && activeCreature.DMControl)
                {
                    RollInitiative(activeCreature);
                }
            }
            OrderInitiative(activeEncounter);
            activeEncounter.ActiveTurn = activeEncounter.CreatureTurns.Peek();
        }
        else if (activeEncounter.CreatureTurns.Contains(activeEncounter.ActiveTurn) && !activeEncounter.ActiveTurn.Dead)
        {
            OrderInitiative(activeEncounter);
            while (activeEncounter.CreatureTurns.Peek() != activeEncounter.ActiveTurn)
            {
                await EndCurrentTurnAsync(activeEncounter);
            }
        }
        else
            throw new ArgumentException("The active turn must be a living creature that exists within the encounter.");
        await _dataService.SaveAddAsync(activeEncounter);

        _logService.StartEncounterLog(activeEncounter.ActiveTurn);

        return activeEncounter.CreatureTurns;
    }

    private static string DamageTypeToString(DamageType damage)
    {
        StringBuilder sb = new();
        foreach (DamageType damageType in Enum.GetValues(typeof(DamageType)))
        {
            if ((damage & damageType) != 0)
                sb.Append(damageType.ToString());
        }
        return sb.ToString();
    }

    private void AddCreatureToInitiativeOrder(ActiveEncounter encounter, ActiveEncounterCreature activeEncounterCreature)
    {
        encounter.CreatureTurns.Enqueue(activeEncounterCreature); //todo:
    }

    private IEnumerable<ActiveEncounterCreature> CreateCreaturesForActiveEncounter(Encounter encounter, Party party)
    {
        List<ActiveEncounterCreature> activeCreatures = new();

        foreach (var creature in encounter.Creatures)
        {
            activeCreatures.Add(_encounterService.CreateActiveEncounterCreature(creature, RollHP));
        }
        foreach (var creature in party.Members)
            if (creature.DMControl)
                activeCreatures.Add(_encounterService.CreateActiveEncounterCreature(creature, false));

        foreach (var creature in party.Members)
            if (!creature.DMControl)
                activeCreatures.Add(_encounterService.CreateActiveEncounterCreature(creature, false));

        return activeCreatures;
    }

    private async Task<string> DealDamageAsync(ActiveEncounter activeEncounter, ActiveEncounterCreature sourceCreature, ActiveEncounterCreature targetCreature, DamageType damageType, int damageValue)
    {
        //var logMessageString = "";
        if (damageType == DamageType.Healing)
        {
            targetCreature.CurrentHP += damageValue;
            if (targetCreature.CurrentHP > targetCreature.MaxHP)
                targetCreature.CurrentHP = targetCreature.MaxHP;

            //logMessageString = $"{sourceCreature.EncounterName} healed {targetCreature.EncounterName} for {damageValue}";
        }
        else
        {
            targetCreature.CurrentHP -= damageValue;

            if (targetCreature.MaxHP != 0 && targetCreature.CurrentHP <= 0) //non-pc character dead'd
            {
                targetCreature.Dead = true;
                //logMessageString = $"{sourceCreature.EncounterName} deals {damageValue} {DamageTypeToString(damageType)} " +
                //    $"to {targetCreature.EncounterName} \n {targetCreature.EncounterName} is downed! {targetCreature.CurrentHP} overkill.";
            }
            //else
            //    logMessageString = $"{sourceCreature.EncounterName} deals {damageValue} {DamageTypeToString(damageType)} to {targetCreature.EncounterName}";
        }
        //activeEncounter.Log.Add(logMessageString);
        //return logMessageString;
        return await _logService.LogDamage(damageType, sourceCreature, targetCreature, damageValue);
    }

    private int GetEffectiveDamage(int damage, DamageVolume multiplierEnum)
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
            //you halve twice, rounding down each time.
            result /= 2;
            result /= 2;
        }
        return result;
    }

    private void OrderInitiative(ActiveEncounter activeEncounter)
    {
        if (activeEncounter.ActiveCreatures.Count == 0)
            return;
        activeEncounter.CreatureTurns.Clear();
        //clean(ish) tiebreaking by roll, then dex bonus, then finally if PC or NPC (advantage to PC)
        var ordered = activeEncounter.ActiveCreatures
                                                    .OrderByDescending(x => x.Initiative + ((double)_creatureService.GetAttributeBonusValue(x, CreatureAttributeType.Dexterity) / 100))
                                                    .ThenByDescending(y => y.DMControl);

        foreach (var creature in ordered)
        {
            activeEncounter.CreatureTurns.Enqueue(creature);
        }
    }

    private void RollInitiative(ActiveEncounterCreature activeEncounterCreature)
    {
        activeEncounterCreature.Initiative = _randomService.RandomInteger(1, 20)
            + activeEncounterCreature.InitiativeBonus
            + _creatureService.GetAttributeBonusValue(activeEncounterCreature, CreatureAttributeType.Dexterity);

        if (activeEncounterCreature.InitiativeAdvantage)
        {
            var adv = _randomService.RandomInteger(1, 20)
                + activeEncounterCreature.InitiativeBonus
                + _creatureService.GetAttributeBonusValue(activeEncounterCreature, CreatureAttributeType.Dexterity);

            if (activeEncounterCreature.Initiative < adv)
                activeEncounterCreature.Initiative = adv;
        }
    }
}