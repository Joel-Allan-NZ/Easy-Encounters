using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEncounters.Core.Models.Enums;
[Flags]
public enum DamageType
{
    None = 0,
    Acid = 1 << 0,
    Bludgeoning = 1 << 1,
    Cold = 1 << 2,
    Fire = 1 << 3,
    Force = 1 << 4,
    Lightning = 1 << 5,
    Necrotic = 1 << 6,
    Piercing = 1 << 7,
    Poison = 1 << 8,
    Psychic = 1 << 9,
    Radiant = 1 << 10,
    Slashing = 1 << 11,
    Thunder = 1 << 12,
    NonSilveredPhysical = 1 << 13, //some creatures have resistance or immunity against physical damage from non-silvered, non-magical weapons
    NonMagicalPhysical = 1 << 14, //some creatures have resistance or immunity against physical damage from non-magical sources.
    Healing = 1 << 15,


}
