using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Core.Models.Logs;

namespace EasyEncounters.Core.Services;
public class LogService : ILogService
{
    private static readonly string folderPath = @"D:\D&D\DND Tools";
    private static readonly string errorfileName = @"ErrorLogging.txt";
    private static readonly string fileName = @"Log.txt";
    private readonly IDataService _dataService;
    private readonly IFileService _fileService;
    private Log _cached;



    private EncounterLog _encounterLog;

    public LogService(IDataService dataService, IFileService fileService)
    {
        _dataService = dataService;
        _fileService = fileService;
        _cached = _fileService.Read<Log>(folderPath, fileName) ?? new Log(null);
    }

    public async Task SaveAsync()
    {
        await _fileService.SaveAsync(folderPath, fileName, _cached);
    }
    public void LogError(string message)
    {
        _fileService.SaveAsync(folderPath, errorfileName, message);
    }

    public void StartEncounterLog(ActiveEncounterCreature firstTurnCreature)
    {
        _encounterLog = new EncounterLog();
        LogTurnStart(firstTurnCreature);
    }

    public string LogTurnStart(ActiveEncounterCreature creature)
    {
        _encounterLog.Turns.Add(new TurnLog(creature));

        return $"[{_encounterLog.Turns.Last().TurnStart.ToString("HH:mm:ss")}]: {creature.EncounterName}'s turn.";
    }

    public string LogTurnEnd()
    {
        var endingTurn = _encounterLog.Turns.Last();
        if(endingTurn.TurnEnd == null)
            endingTurn.TurnEnd = DateTime.Now;
        //encounterLog.Turns.Last().TurnEnd = DateTime.Now;

        return $"[{endingTurn.TurnEnd?.ToString("HH:mm:ss") ?? DateTime.Now.ToString("HH:mm:ss")}]: {endingTurn.ActiveTurnCreature.EncounterName} ends its turn.";
    }

    public async Task EndEncounterLog()
    {
        LogTurnEnd();
        foreach (var turn in _encounterLog.Turns)
        {
            if (!turn.ActiveTurnCreature.DMControl)
            {
                var seconds = ((turn.TurnEnd ??= DateTime.Now) - turn.TurnStart).TotalSeconds;
                if (!_cached.CharacterLogs.ContainsKey(turn.ActiveTurnCreature.EncounterName))
                    _cached.CharacterLogs[turn.ActiveTurnCreature.EncounterName] = new CharacterData();

                _cached.CharacterLogs[turn.ActiveTurnCreature.EncounterName].TurnSecondsTaken += seconds;
            }
        }
        await SaveAsync();
    }

    public async Task<string> LogDamage(DamageType damageType, ActiveEncounterCreature source, ActiveEncounterCreature target, int damage)
    {
        if (!source.DMControl)
        {
            if (!_cached.CharacterLogs.ContainsKey(source.EncounterName))
            {
                _cached.CharacterLogs[source.EncounterName] = new CharacterData();
            }
            if (!_cached.CharacterLogs[source.EncounterName].DamageDealt.ContainsKey(damageType))
            {
                _cached.CharacterLogs[source.EncounterName].DamageDealt[damageType] = 0.0;
            }
            _cached.CharacterLogs[source.EncounterName].DamageDealt[damageType] += damage;
        }
        if (!target.DMControl)
        {
            if (!_cached.CharacterLogs.ContainsKey(target.EncounterName))
            {
                _cached.CharacterLogs[target.EncounterName] = new CharacterData();
            }
            if (!_cached.CharacterLogs[target.EncounterName].DamageTaken.ContainsKey(damageType))
            {
                _cached.CharacterLogs[target.EncounterName].DamageTaken[damageType] = 0.0;
            }
            _cached.CharacterLogs[target.EncounterName].DamageTaken[damageType] += damage;
        }

        await SaveAsync();
        if(damageType == DamageType.Healing)
        {
            return $"[{DateTime.Now.ToString("HH:mm:ss")}]: {source.EncounterName} healed {target.EncounterName} for {damage}";
        }
        else
            return $"[{DateTime.Now.ToString("HH:mm:ss")}]: {source.EncounterName} deals {damage} {damageType} to {target.EncounterName}";
    }

    private void AddTurnTime(TurnLog turn)
    {
        if (!turn.ActiveTurnCreature.DMControl)
        {
            if (!_cached.CharacterLogs.ContainsKey(turn.ActiveTurnCreature.Name))
            {
                _cached.CharacterLogs.Add(turn.ActiveTurnCreature.Name, new CharacterData());
            }
            _cached.CharacterLogs[turn.ActiveTurnCreature.Name].TurnSecondsTaken += ((turn.TurnEnd??= DateTime.Now) - turn.TurnStart).TotalSeconds;
        }
    }
}
