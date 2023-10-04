using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using EasyEncounters.Contracts.Services;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Helpers;
using Microsoft.UI.Xaml;

using Windows.ApplicationModel;

namespace EasyEncounters.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly IModelOptionsService _modelOptionsService;

    [ObservableProperty]
    private ElementTheme _elementTheme;

    [ObservableProperty]
    private string _versionDescription;

    [ObservableProperty]
    private bool _rollMaxHP;

    async partial void OnRollMaxHPChanged(bool value)
    {
        await _modelOptionsService.SaveActiveEncounterOptionAsync(value);
    }

    //public ICommand SwitchRollMaxCommand
    //{
    //    get;
    //}

    public ICommand SwitchThemeCommand
    {
        get;
    }

    public SettingsViewModel(IThemeSelectorService themeSelectorService, IModelOptionsService modelOptionsService)
    {
        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();      
        _modelOptionsService = modelOptionsService;
        _rollMaxHP = _modelOptionsService.RollHP;

        //SwitchRollMaxCommand = new RelayCommand<bool>(
        //    async (param) =>
        //    {
        //        if(RollMaxHP != param)
        //        {
        //            RollMaxHP = param;
        //            await _modelOptionsService.SaveActiveEncounterOptionAsync(param);
        //        }
        //    });

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
}
