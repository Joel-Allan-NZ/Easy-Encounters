using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEncounters.Core.Models.Enums;

[Flags]
public enum CreatureAttributeType
{
    None = 0,
    Strength = 1 << 0,
    Dexterity = 1 << 1,
    Constitution = 1 << 2,
    Intelligence = 1 << 3,
    Wisdom = 1 << 4,
    Charisma = 1 << 5, 

}
