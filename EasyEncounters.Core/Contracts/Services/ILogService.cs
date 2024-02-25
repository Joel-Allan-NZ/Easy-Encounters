using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Contracts.Services;

public interface ILogService
{
    Task EndEncounterLog();
    Task<string> LogDamage(DamageType damageType, ActiveEncounterCreature source, ActiveEncounterCreature target, int damage);

    void LogError(string message);

    string LogTurnEnd();

    string LogTurnStart(ActiveEncounterCreature creature);

    Task SaveAsync();

    void StartEncounterLog(ActiveEncounterCreature firstTurnCreature);
}