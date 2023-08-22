using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Contracts;

namespace EasyEncounters.Core.Models;
/// <summary>
/// A base class for model classes that can be persisted.
/// </summary>
public class Persistable : IPersistable
{
    public Guid Id
    {
        get; set;
    }
}
