using Microsoft.UI.Xaml.Controls;

namespace EasyEncounters.Contracts.Services;

public interface INavigationViewService
{
    IList<object>? MenuItems
    {
        get;
    }

    object? SettingsItem
    {
        get;
    }

    NavigationViewItem? GetSelectedItem(Type pageType);

    void Initialize(NavigationView navigationView);

    void UnregisterEvents();
}