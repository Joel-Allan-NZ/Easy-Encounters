using EasyEncounters.Core.Models;

namespace EasyEncounters.Core.Contracts.Services;

public interface IAbilityService
{
    void CopyTo(Ability copyTarget, Ability copySource);

    /// <summary>
    /// Creates a version of a given ability that reflects the stats of a given creature,
    /// giving a correct bonus to hit or SaveDC
    /// </summary>
    /// <param name="creature"></param>
    /// <param name="ability"></param>
    /// <param name="abilityStatValue"></param>
    /// <returns></returns>
    ActiveAbility CreateActiveAbility(ActiveEncounterCreature creature, Ability ability, int abilityStatValue);
}