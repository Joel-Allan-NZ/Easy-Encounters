namespace EasyEncounters.Core.Models.Logs;

public class Log
{
    public Log(Dictionary<string, CharacterData> characterLogs)
    {
        CharacterLogs = characterLogs ?? new Dictionary<string, CharacterData>();
    }

    public Dictionary<string, CharacterData> CharacterLogs
    {
        get; set;
    }
}