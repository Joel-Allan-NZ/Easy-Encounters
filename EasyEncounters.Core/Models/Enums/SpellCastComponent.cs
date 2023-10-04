namespace EasyEncounters.Core.Models.Enums;

[Flags]
public enum SpellCastComponent
{
    None = 0,
    Verbal = 1 << 0,
    Somatic = 1 << 1,
    Material = 1 << 2,
}