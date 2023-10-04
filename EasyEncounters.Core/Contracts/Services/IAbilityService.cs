using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Models;

namespace EasyEncounters.Core.Contracts.Services;
public interface IAbilityService
{
    ActiveAbility CreateActiveAbility(ActiveEncounterCreature creature, Ability ability, int abilityStatValue);

    void CopyTo(Ability copyTarget, Ability copySource);
}
