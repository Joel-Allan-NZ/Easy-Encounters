using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Core.Contracts.Services;
public interface ILogService
{
    Task SaveAsync();
    Task EndEncounterLog();

    string LogTurnStart(ActiveEncounterCreature creature);

    string LogTurnEnd();
    //void Log(string message);
    Task<string> LogDamage(DamageType damageType, ActiveEncounterCreature source, ActiveEncounterCreature target, int damage);
    void StartEncounterLog(ActiveEncounterCreature firstTurnCreature);
    void LogError(string message);
}
