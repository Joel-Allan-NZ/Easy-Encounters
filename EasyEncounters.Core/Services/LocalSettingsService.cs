
using System.Drawing.Drawing2D;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Helpers;
using EasyEncounters.Core.Services.Models;


using Microsoft.Extensions.Options;
//using Windows.Storage;

namespace EasyEncounters.Core.Services;

public class LocalSettingsService : ILocalSettingsService
{
    private const string _defaultApplicationDataFolder = "EasyEncounters\\ApplicationData";
    private const string _defaultLocalSettingsFile = "LocalSettings.json";

    private readonly string _applicationDataFolder;
    private readonly IFileService _fileService;
    private readonly string _localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private readonly string _localsettingsFile;
    private readonly LocalSettingsOptions _options;
    private bool _isInitialized;
    private LocalSettings? _settings;

    public LocalSettingsService(IFileService fileService, IOptions<LocalSettingsOptions> options)
    {
        _fileService = fileService;
        _options = options.Value;

       // _applicationDataFolder = 

        _applicationDataFolder = Path.Combine(_localApplicationData, _options.ApplicationDataFolder ?? _defaultApplicationDataFolder);
        _localsettingsFile = _options.LocalSettingsFile ?? _defaultLocalSettingsFile;

        //_settings = new Dictionary<string, object>();
    }

    public async Task<T> ReadSettingAsync<T>(string key)
    {
        if (RuntimeHelper.IsMSIX)
        {
            _settings = await _fileService.ReadAsync<LocalSettings>(_applicationDataFolder, _localsettingsFile) ?? CreateDefaultSettings();

            if (_settings.Settings.TryGetValue(key, out var obj))
            {
                return (T)obj;
            }
        }
        else
        {
            await InitializeAsync();

            if (_settings != null && _settings.Settings.TryGetValue(key, out var obj))
            {
                return await Json.ToObjectAsync<T>((string)obj);
            }
        }

        return default;
    }

    public async Task SaveSettingAsync<T>(string key, T value)
    {
        if (RuntimeHelper.IsMSIX)
        {
            _settings = await _fileService.ReadAsync<LocalSettings>(_applicationDataFolder, _localsettingsFile) ?? CreateDefaultSettings();
            _settings.Settings[key] = value;

            await _fileService.SaveAsync(_applicationDataFolder, _localsettingsFile, _settings);
        }
        else
        {
            await InitializeAsync();

            _settings.Settings[key] = await Json.StringifyAsync(value);

            await _fileService.SaveAsync(_applicationDataFolder, _localsettingsFile, _settings);
        }
    }

    private async Task InitializeAsync()
    {
        if (!_isInitialized)
        {
            _settings.Settings = await Task.Run(() => _fileService.Read<Dictionary<string, object>>(_applicationDataFolder, _localsettingsFile)) ?? new Dictionary<string, object>();

            _isInitialized = true;
        }
    }

    private LocalSettings CreateDefaultSettings()
    {
        return new LocalSettings()
        {
            Settings = new(){
                { "RollHP", false },
                { "SavePath", _applicationDataFolder }
            }
        };
    }
}