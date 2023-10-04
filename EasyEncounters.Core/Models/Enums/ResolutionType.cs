using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Schema;

namespace EasyEncounters.Core.Models.Enums;
public enum ResolutionType
{
    Undefined =0,
    Attack = 1,
    SavingThrow = 2,
    WillingTarget = 4,
    SpellAttack = 8
}
