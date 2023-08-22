using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEncounters.Core.Models.Enums;
public enum DamageVolume
{
    None = 0,
    Quarter = 1, //saving for half against something you have resistance for
    Half = 2, //save for half, or resist
    Normal = 3,
    Double = 4 //vuln
}
