using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEncounters.Contracts.Services;
public interface IModelOptionsService
{
    bool RollHP
    {
        get;
    }

    Task<bool> ReadActiveEncounterOptionAsync();

    Task SaveActiveEncounterOptionAsync(bool optionValue);
}
