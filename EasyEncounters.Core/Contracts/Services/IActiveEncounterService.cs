using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Contracts.Services;

public interface IActiveEncounterService
{
    /// <summary>
    /// Adds an ActiveEncounterCreature based on selected creature to the ActiveEncounter, ensuring its EncounterName is unique.
    /// </summary>
    /// <param name="inProgress"></param>
    /// <param name="creature"></param>
    Task AddCreatureToInProgressEncounterAsync(ActiveEncounter inProgress, Creature creature);

    /// <summary>
    /// Creates an ActiveEncounter from an Encounter for a given Party,
    /// creating ActiveEncounterCreatures along the way.
    /// </summary>
    /// <param name="encounter"></param>
    /// <param name="party"></param>
    /// <returns></returns>
    Task<ActiveEncounter> CreateActiveEncounterAsync(Encounter encounter, Party party);

    Task<string> DealDamageAsync(ActiveEncounter activeEncounter, DamageInstance damageInstance);

    /// <summary>
    /// Ends the current turn, moving control to the next creature's turn and performing start-of-turn clean up
    /// </summary>
    /// <param name="activeEncounter"></param>
    /// <returns></returns>
    Task<string> EndCurrentTurnAsync(ActiveEncounter activeEncounter);

    Task EndEncounterAsync(ActiveEncounter activeEncounter);

    /// <summary>
    /// For a given damage type, get a suggestion as to what damage volume multiplier should be applied to it for a given creature.
    /// For example, for a creature with Fire Resistance return a suggestion that fire damage should be halved
    /// </summary>
    /// <param name="target"></param>
    /// <param name="damageType"></param>
    /// <returns></returns>
    DamageVolume GetDamageVolumeSuggestion(ActiveEncounterCreature target, DamageType damageType);

    /// <summary>
    /// Force the ActiveEncounter to have a new turn order for creatures
    /// </summary>
    /// <param name="activeEncounter"></param>
    /// <param name="creatures"></param>
    void OrderCreatureTurns(ActiveEncounter activeEncounter, IEnumerable<ActiveEncounterCreature> turnOrder);

    /// <summary>
    /// Updates the initiative order, automatically rolling initiative for any DM controlled creatures without a current initiative roll.
    /// </summary>
    /// <param name="activeEncounter"></param>
    /// <returns>The ActiveEncounterCreatures in the ActiveEncounter, returned in initiative order</returns>
    Task<IEnumerable<ActiveEncounterCreature>> RollInitiative(ActiveEncounter activeEncounter);

    Task<ActiveEncounter> FullyLoadActiveEncounter(ActiveEncounter encounter);

}