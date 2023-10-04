using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Contracts.Services;
public interface IConditionService
{
    string GetConditionDescription(Condition condition);

    void AddCondition(ActiveEncounterCreature creature, Condition condition);
}
