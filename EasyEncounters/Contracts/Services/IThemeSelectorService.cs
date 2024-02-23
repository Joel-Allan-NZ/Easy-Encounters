using Microsoft.UI.Xaml;

namespace EasyEncounters.Contracts.Services;

public interface IThemeSelectorService
{
    ElementTheme Theme
    {
        get;
    }

    Task InitializeAsync();

    Task SetRequestedThemeAsync();

    Task SetThemeAsync(ElementTheme theme);
}