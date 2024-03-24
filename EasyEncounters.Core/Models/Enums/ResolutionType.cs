namespace EasyEncounters.Core.Models.Enums;

/// <summary>
/// The way an ability is resolved - can it affect only a willing target, is there a saving throw etc
/// </summary>
public enum ResolutionType
{
    Undefined = 0,
    Attack = 1,
    SavingThrow = 2,
    WillingTarget = 4,
    SpellAttack = 8
}