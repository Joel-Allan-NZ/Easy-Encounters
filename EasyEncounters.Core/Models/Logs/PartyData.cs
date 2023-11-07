using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEncounters.Core.Models.Logs;
public class PartyData
{
    public Dictionary<string, CharacterData> CharacterLogs
    {
        get; set;
    }

    public PartyData(Dictionary<string, CharacterData> characterLogs = null)
    {
        CharacterLogs = characterLogs ?? new Dictionary<string, CharacterData>();
    }
}
