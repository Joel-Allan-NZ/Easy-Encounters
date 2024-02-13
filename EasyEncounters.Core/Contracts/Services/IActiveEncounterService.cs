using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    Task<string> DealDamageAsync(ActiveEncounter activeEncounter, DamageInstance damageInstance);

    DamageVolume GetDamageVolumeSuggestion(ActiveEncounterCreature target, DamageType damageType);


    Task EndEncounterAsync(ActiveEncounter activeEncounter);

    /// <summary>
    /// Updates the initiative order, automatically rolling initiative for any DM controlled creatures without a current initiative roll.
    /// </summary>
    /// <param name="activeEncounter"></param>
    /// <returns>The ActiveEncounterCreatures in the ActiveEncounter, returned in initiative order</returns>
    Task<IEnumerable<ActiveEncounterCreature>> UpdateInitiativeOrder(ActiveEncounter activeEncounter);

    Task<string> EndCurrentTurnAsync(ActiveEncounter activeEncounter);

    Task<ActiveEncounter> CreateActiveEncounterAsync(Encounter encounter, Party party);

    void AddEncounterCreature(ActiveEncounter activeEncounter, Creature creature, bool maxHPRoll);

    void ReorderInitiative(ActiveEncounter activeEncounter, IEnumerable<ActiveEncounterCreature> creatures);

    /// <summary>
    /// Adds an ActiveEncounterCreature based on selected creature to the ActiveEncounter, ensuring its EncounterName is unique.
    /// </summary>
    /// <param name="activeEncounter"></param>
    /// <param name="creature"></param>
    void AddCreatureInProgress(ActiveEncounter activeEncounter, Creature creature);
}
