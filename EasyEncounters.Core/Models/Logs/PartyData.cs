namespace EasyEncounters.Core.Models.Logs;

public class PartyData
{
    public PartyData(Dictionary<string, CharacterData> characterLogs = null)
    {
        CharacterLogs = characterLogs ?? new Dictionary<string, CharacterData>();
    }

    public Dictionary<string, CharacterData> CharacterLogs
    {
        get; set;
    }
}