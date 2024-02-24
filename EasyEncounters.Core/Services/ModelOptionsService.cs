//using EasyEncounters.Contracts.Services;
using EasyEncounters.Core.Contracts.Services;
//using Windows.Storage.Search;

namespace EasyEncounters.Services;

public class ModelOptionsService : IModelOptionsService
{
    private const string OptionsKey = "RollHP";
    private const string FolderPath = "SavePath";
    private readonly ILocalSettingsService _localSettingService;

    public ModelOptionsService(ILocalSettingsService localSettingService)
    {
        _localSettingService = localSettingService;
    }

    public bool RollHP
    {
        get; private set;
    }

    public string? SavePath
    {
        get; private set;
    }

    public async Task Initialize()
    {
        await ReadActiveEncounterOptionAsync();
        await ReadSaveLocation();
    }

    public async Task ReadActiveEncounterOptionAsync()
    {
        RollHP = await _localSettingService.ReadSettingAsync<bool>(OptionsKey);
    }

    public async Task ReadSaveLocation()
    {
        SavePath = await _localSettingService.ReadSettingAsync<string>(FolderPath);
        
    }

    public async Task SaveActiveEncounterOptionAsync(bool optionValue)
    {
        await _localSettingService.SaveSettingAsync<bool>(OptionsKey, optionValue);
        RollHP = optionValue;
    }

    public async Task SaveFolderPath(string path)
    {
        SavePath = path;
        await _localSettingService.SaveSettingAsync(FolderPath, path);
    }
}