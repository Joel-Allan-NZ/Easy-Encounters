namespace EasyEncounters.Core.Contracts.Services;

public interface IModelOptionsService
{
    bool RollHP
    {
        get;
    }

    string SavePath
    {
        get;
    }

    Task Initialize();

    Task ReadActiveEncounterOptionAsync();

    Task SaveActiveEncounterOptionAsync(bool optionValue);

    Task SaveFolderPath(string path);

    Task ReadSaveLocation();
}