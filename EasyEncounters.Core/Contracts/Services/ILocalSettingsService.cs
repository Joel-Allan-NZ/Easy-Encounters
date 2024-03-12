namespace EasyEncounters.Core.Contracts.Services;
#nullable enable
public interface ILocalSettingsService
{
    Task<T?> ReadSettingAsync<T>(string key);

    Task SaveSettingAsync<T>(string key, T value);
}