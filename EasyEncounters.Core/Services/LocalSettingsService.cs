﻿using System.Reflection;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Helpers;
using EasyEncounters.Core.Services.Models;


using Microsoft.Extensions.Options;
//using Windows.Storage;

namespace EasyEncounters.Core.Services;
#nullable enable
public class LocalSettingsService : ILocalSettingsService
{
    private const string _defaultApplicationDataFolder = "EasyEncounters";
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
        CopyDB();
        _applicationDataFolder = Path.Combine(_localApplicationData, _defaultApplicationDataFolder?? _options.ApplicationDataFolder);
        _localsettingsFile = _options.LocalSettingsFile ?? _defaultLocalSettingsFile;

        //_settings = new Dictionary<string, object>();
    }

    private void CopyDB()
    {

        string result = Assembly.GetExecutingAssembly().Location;
        int index = result.LastIndexOf("\\");
        string dbPath = $"{result.Substring(0, index)}\\EasyEncounters.db";


        
        string destinationFolder = Path.Combine(_localApplicationData, _options.ApplicationDataFolder ?? _defaultApplicationDataFolder); //$"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\EasyEncounters\\";
        string destinationPath = Path.Combine(destinationFolder, "EasyEncounters.db");

        if (!File.Exists(dbPath))
        {
            string nope = "nope, not a file path match";
            var filePathDir = Path.Combine(destinationFolder, "nope.txt");
            File.WriteAllText(filePathDir, nope);
        }
        if (!File.Exists(destinationPath))
        {
            Directory.CreateDirectory(destinationFolder);
            File.Copy(dbPath, destinationPath, true);
            var settings = "LocalSettings.json";
            var oldLog = "Logging.txt";
            var error = "ErrorLogging.txt";
            var log = "Log.txt";

            File.Copy(Path.Combine($"{result.Substring(0, index)}\\", settings), Path.Combine(destinationFolder, settings), true);
            File.Copy(Path.Combine($"{result.Substring(0, index)}\\", oldLog), Path.Combine(destinationFolder, oldLog), true);
            File.Copy(Path.Combine($"{result.Substring(0, index)}\\", error), Path.Combine(destinationFolder, error), true);
            File.Copy(Path.Combine($"{result.Substring(0, index)}\\", log), Path.Combine(destinationFolder, log), true);


        }


    }

    public async Task<T?> ReadSettingAsync<T>(string key)
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

            if (_settings != null)
            {
                _settings.Settings[key] = await Json.StringifyAsync(value);

                await _fileService.SaveAsync(_applicationDataFolder, _localsettingsFile, _settings);
            }

        }
    }

    private async Task InitializeAsync()
    {
        if (!_isInitialized)
        {
            _settings ??= new LocalSettings();

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