using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace EasyEncounters.Core.Services;
public class ActiveEncounterService : IActiveEncounterService
{
    readonly IRandomService _randomService;
    readonly IDataService _dataService;
    readonly IEncounterService _encounterService;
    public ActiveEncounterService(IRandomService randomService, IDataService dataService, IEncounterService encounterService)
    {
        _encounterService = encounterService;
        _randomService = randomService;
        _dataService = dataService;

    }

    public async Task<ActiveEncounter> CreateActiveEncounterAsync(Encounter encounter, Party party)
    {
        var active = new ActiveEncounter(encounter, party);
        await _dataService.SaveAddAsync(active);
        OrderInitiative(active);
        return active;
    }

    public void AddEncounterCreature(ActiveEncounter activeEncounter, Creature creature, bool maxHPRoll)
    {
        HashSet<string> names = new HashSet<string>();
        foreach (var activeCreature in activeEncounter.ActiveCreatures)
            names.Add(activeCreature.Name);

        var tempCreature = _encounterService.CreateActiveEncounterCreature(creature, maxHPRoll);
        int collisions = 0;

        while (names.Contains(tempCreature.EncounterName))
        {
            collisions++;
            tempCreature.EncounterName = $"{tempCreature.Name} {collisions}";
        }

        activeEncounter.ActiveCreatures.Add(tempCreature); 
    }

    public void DealDamage(ActiveEncounter activeEncounter, ActiveEncounterCreature sourceCreature, ActiveEncounterCreature targetCreature, DamageType damageType, int damageValue)
    {
        if(damageType == DamageType.Healing)
        {
            targetCreature.CurrentHP += damageValue;
            if (targetCreature.CurrentHP > targetCreature.MaxHP)
                targetCreature.CurrentHP = targetCreature.MaxHP;

            activeEncounter.Log.Add($"{sourceCreature.EncounterName} healed {targetCreature.EncounterName} for {damageValue}");
        }
        else
        {
            targetCreature.CurrentHP -= damageValue;
            activeEncounter.Log.Add($"{sourceCreature.EncounterName} deals {damageValue} {DamageTypeToString(damageType)} to {targetCreature.EncounterName}");
        }
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

    public async Task EndCurrentTurnAsync(ActiveEncounter activeEncounter)
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
        activeEncounter.ActiveTurn.Reaction = true;

        await _dataService.SaveAddAsync(activeEncounter);
    }

    public async Task EndEncounterAsync(ActiveEncounter activeEncounter)
    {
       await _dataService.WriteLog(activeEncounter.Log);
        await _dataService.ClearActiveEncounterAsync();
    }

    private void RollInitiative(ActiveEncounterCreature activeEncounterCreature)
    {

        activeEncounterCreature.Initiative = _randomService.RandomInteger(1, 20) + activeEncounterCreature.InitiativeBonus;
        if (activeEncounterCreature.InitiativeAdvantage)
        {
            var adv = _randomService.RandomInteger(1, 20) + activeEncounterCreature.InitiativeBonus;
            if (activeEncounterCreature.Initiative < adv)
                activeEncounterCreature.Initiative = adv;
        }
    }

    public async Task StartEncounterAsync(ActiveEncounter activeEncounter)
    {
        foreach(var activeCreature in activeEncounter.ActiveCreatures)
        {
            if (activeCreature.Initiative == 0 && activeCreature.DMControl)
            {
                RollInitiative(activeCreature);
            }
        }
        OrderInitiative(activeEncounter);
        activeEncounter.ActiveTurn = activeEncounter.CreatureTurns.Peek();

        await _dataService.SaveAddAsync(activeEncounter);
    }

    private void OrderInitiative(ActiveEncounter activeEncounter)
    {
        if (activeEncounter.ActiveCreatures.Count == 0)
            return;
        activeEncounter.CreatureTurns.Clear();
        //clean(ish) tiebreaking by roll, then dex bonus, then finally if PC or NPC (advantage to PC)
        var ordered = activeEncounter.ActiveCreatures.OrderByDescending(x => x.Initiative + ((double)x.DexBonus / 100)).ThenBy(y => y.DMControl);

        foreach (var creature in ordered)
        {
            activeEncounter.CreatureTurns.Enqueue(creature);
        }
        
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

    public void DealDamage(ActiveEncounter activeEncounter, DamageInstance damageInstance)
    {
        DealDamage(activeEncounter, damageInstance.Source, damageInstance.Target, damageInstance.DamageType, GetEffectiveDamage(damageInstance.BaseDamageValue, damageInstance.DamageVolume));
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
}
