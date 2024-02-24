using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using EasyEncounters.Contracts.Services;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Helpers;
using Microsoft.UI.Xaml;

using Windows.ApplicationModel;
using Windows.Storage.Pickers;

namespace EasyEncounters.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IModelOptionsService _modelOptionsService;
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ElementTheme _elementTheme;

    [ObservableProperty]
    private bool _rollMaxHP;

    [ObservableProperty]
    private string _versionDescription;

    [ObservableProperty]
    private string _locationDescription;

    public SettingsViewModel(IThemeSelectorService themeSelectorService, IModelOptionsService modelOptionsService, INavigationService navigationService)
    {
        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();
        _modelOptionsService = modelOptionsService;
        _rollMaxHP = _modelOptionsService.RollHP;
        LocationDescription = _modelOptionsService.SavePath;
        _navigationService = navigationService;

        SwitchThemeCommand = new RelayCommand<ElementTheme>(
            async (param) =>
            {
                if (ElementTheme != param)
                {
                    ElementTheme = param;
                    await _themeSelectorService.SetThemeAsync(param);
                }
            });
    }

    public ICommand SwitchThemeCommand
    {
        get;
    }

    [RelayCommand]
    private async Task PickFolder()
    {
        var hwnd = _navigationService.GetWindowHandle();

        var folderPicker = new FolderPicker();

        WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);

        var result = await folderPicker.PickSingleFolderAsync();

        if (!string.IsNullOrEmpty(result.Path))
        {
            await _modelOptionsService.SaveFolderPath(result.Path);
        }

    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    async partial void OnRollMaxHPChanged(bool value)
    {
        await _modelOptionsService.SaveActiveEncounterOptionAsync(value);
    }
}