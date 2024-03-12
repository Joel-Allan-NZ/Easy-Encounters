namespace EasyEncounters.Core.Models.Enums;

/// <summary>
/// The alignment of a creature in 5e
/// </summary>
[Flags]
public enum CreatureAlignment : int
{
    Undefined = 0,
    LawfulGood = 1 << 0,
    LawfulNeutral = 1 << 1,
    LawfulEvil = 1 << 2,
    NeutralGood =1 << 3,
    TrueNeutral =1 << 4,
    NeutralEvil =1 << 5,
    ChaoticGood =1 << 6,
    ChaoticNeutral =1 << 7,
    ChaoticEvil = 1 << 8,

}