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