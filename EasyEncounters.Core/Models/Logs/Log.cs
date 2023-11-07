using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEncounters.Core.Models.Logs;
public class Log
{
    public Dictionary<string, CharacterData> CharacterLogs
    {
        get; set;
    }
    public Log(Dictionary<string, CharacterData> characterLogs)
    {
        CharacterLogs = characterLogs ?? new Dictionary<string, CharacterData>();
    }
}
