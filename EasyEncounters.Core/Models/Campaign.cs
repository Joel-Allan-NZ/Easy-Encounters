using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEncounters.Core.Models;
/// <summary>
/// For dividing encounters into campaign blocks.
/// </summary>
public class Campaign : Persistable
{
    public string Name
    {
        get; set;
    }

    public string Description
    {
        get; set;
    }

    public Campaign(string name = "New Campaign", string desc = "Add a description.")
    {
        Name = name;
        Id = Guid.NewGuid();
        Description = desc;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is Campaign))
        {
            return false;
        }
        return ((Campaign)obj).Id == Id;
    }
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}