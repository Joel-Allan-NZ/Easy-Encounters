using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Contracts;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Models;
public class Damage : IPersistable
{
    Guid IPersistable.Id
    {
        get; set;
    }

    public DamageType DamageType
    {
        get; set;
    }

    /// <summary>
    /// Probably won't use this much? Mostly a placeholder for future damage
    /// strings etc once that grammar system is enacted.
    /// </summary>
    public string Description
    {
        get; set;
    }
}
