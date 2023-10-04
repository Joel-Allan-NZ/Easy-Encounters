using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEncounters.Core.Models.Enums;
[Flags]
public enum Condition
{
    None  = 0,
    Blinded = 1 << 0,
    Charmed = 1 << 1,
    Deafened = 1 << 2,
    Frightened = 1 << 3,
    Grappled = 1 << 4,
    Incapacitated = 1 << 5,
    Invisible = 1 << 6,
    Paralyzed = 1 << 7,
    Petrified = 1 << 8,
    Poisoned = 1 << 9,
    Prone = 1 << 10,
    Restrained = 1 << 11,
    Stunned = 1 << 12,
    Unconscious = 1 << 13,
    Exhausted = 1 << 14,
    All = (1 << 15) - (1<<6) -1,
    Exhaustion2 = 1 << 15,
    Exhaustion3 = 1 << 16,
    Exhaustion4 = 1 << 17,
    Exhaustion5 = 1 << 18,
    Exhaustion6 = 1 << 19,
}
