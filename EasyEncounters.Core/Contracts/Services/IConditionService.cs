using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Contracts.Services;

public interface IConditionService
{
    void AddCondition(ActiveEncounterCreature creature, Condition condition);

    string GetConditionDescription(Condition condition);
}