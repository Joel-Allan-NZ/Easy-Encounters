namespace EasyEncounters.Core.Models.Enums;

/// <summary>
/// The size class of a creature in 5e
/// </summary>
[Flags]
public enum CreatureSizeClass : int
{
    Unknown = 0,
    Tiny = 1 << 0,
    Small = 1 << 1,
    Medium = 1 << 2,
    Large = 1 << 3,
    Huge = 1 << 4,
    Gargantuan = 1 << 5
}