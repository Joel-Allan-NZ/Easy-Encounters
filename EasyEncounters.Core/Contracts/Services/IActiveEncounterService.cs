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
    void DealDamage(ActiveEncounter activeEncounter, ActiveEncounterCreature sourceCreature, ActiveEncounterCreature targetCreature, DamageType damageType, int damageValue);

    void DealDamage(ActiveEncounter activeEncounter, DamageInstance damageInstance);

    DamageVolume GetDamageVolumeSuggestion(ActiveEncounterCreature target, DamageType damageType);

    Task EndEncounterAsync(ActiveEncounter activeEncounter);

    Task StartEncounterAsync(ActiveEncounter activeEncounter);

    Task EndCurrentTurnAsync(ActiveEncounter activeEncounter);

    Task<ActiveEncounter> CreateActiveEncounterAsync(Encounter encounter, Party party);

    void AddEncounterCreature(ActiveEncounter activeEncounter, Creature creature, bool maxHPRoll);
}
