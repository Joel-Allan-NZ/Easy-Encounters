namespace EasyEncounters.Core.Models;

/// <summary>
/// An ActiveAbilty is an Ability that belongs to an ActiveEncounterCreature, with to hit/save DC etc
/// adjusted to reflect that ActiveEncounterCreature. Main use case is Spells
/// </summary>
public class ActiveAbility : Ability
{
    /// <summary>
    /// The total +hit bonus or save DC of an ability, depending on its resolution type.
    /// </summary>
    public int ResolutionValue
    {
        get; set;
    }
}