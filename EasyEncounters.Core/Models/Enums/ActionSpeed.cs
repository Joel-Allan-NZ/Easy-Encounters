namespace EasyEncounters.Core.Models.Enums;

public enum ActionSpeed
{
    None = 0,
    Action = 1 << 0,
    BonusAction = 1 << 1,
    Reaction = 1 << 2,
    Other = 1 << 3,
}