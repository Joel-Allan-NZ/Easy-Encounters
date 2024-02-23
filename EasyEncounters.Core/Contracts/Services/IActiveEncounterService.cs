using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Contracts.Services;

public interface IActiveEncounterService
{
    /// <summary>
    /// Whether or not creatures should have their Max HP rolled rather than using the standard value.
    /// </summary>
    public bool RollHP
    {
        get; set;
    }

    //string DealDamage(ActiveEncounter activeEncounter, ActiveEncounterCreature sourceCreature, ActiveEncounterCreature targetCreature, DamageType damageType, int damageValue);

    /// <summary>
    /// Adds an ActiveEncounterCreature based on selected creature to the ActiveEncounter, ensuring its EncounterName is unique.
    /// </summary>
    /// <param name="inProgress"></param>
    /// <param name="creature"></param>
    void AddCreatureToInProgressEncounter(ActiveEncounter inProgress, Creature creature);

    /// <summary>
    /// Creates an ActiveEncounter from an Encounter for a given Party,
    /// creating ActiveEncounterCreatures along the way.
    /// </summary>
    /// <param name="encounter"></param>
    /// <param name="party"></param>
    /// <returns></returns>
    Task<ActiveEncounter> CreateActiveEncounterAsync(Encounter encounter, Party party);

    Task<string> DealDamageAsync(ActiveEncounter activeEncounter, DamageInstance damageInstance);

    Task<string> EndCurrentTurnAsync(ActiveEncounter activeEncounter);

    Task EndEncounterAsync(ActiveEncounter activeEncounter);

    DamageVolume GetDamageVolumeSuggestion(ActiveEncounterCreature target, DamageType damageType);

    void ReorderInitiative(ActiveEncounter activeEncounter, IEnumerable<ActiveEncounterCreature> creatures);

    /// <summary>
    /// Updates the initiative order, automatically rolling initiative for any DM controlled creatures without a current initiative roll.
    /// </summary>
    /// <param name="activeEncounter"></param>
    /// <returns>The ActiveEncounterCreatures in the ActiveEncounter, returned in initiative order</returns>
    Task<IEnumerable<ActiveEncounterCreature>> UpdateInitiativeOrder(ActiveEncounter activeEncounter);
}