using CommunityToolkit.Mvvm.ComponentModel;

using EasyEncounters.Contracts.Services;
using EasyEncounters.ViewModels;
using EasyEncounters.Views;

using Microsoft.UI.Xaml.Controls;

namespace EasyEncounters.Services;

public class PageService : IPageService
{
    private readonly Dictionary<string, Type> _pages = new();

    public PageService()
    {
        Configure<MainViewModel, MainPage>();
        //Configure<WebViewViewModel, WebViewPage>();
        //Configure<ListDetailsViewModel, ListDetailsPage>();
        Configure<SettingsViewModel, SettingsPage>();
        Configure<CampaignSplashViewModel, CampaignSplashPage>();
        Configure<PartySelectViewModel, PartySelectPage>();
        Configure<RunSessionViewModel, RunSessionPage>();
        Configure<RunEncounterViewModel, RunEncounterPage>();
        //Configure<DealDamageViewModel, DealDamagePage>();
        Configure<CreatureCRUDViewModel, CreatureCRUDPage>();
        Configure<CreatureEditViewModel, CreatureEditPage>();
        Configure<PartyCRUDViewModel, PartyCRUDPage>();
        Configure<PartyEditViewModel, PartyEditPage>();
        Configure<CampaignCRUDViewModel, CampaignCRUDPage>();
        Configure<EncounterCRUDViewModel, EncounterCRUDPage>();
        Configure<EncounterEditViewModel, EncounterEditPage>();
        //Configure<TargetedDamageViewModel, TargetedDamagePage>();
        //Configure<ActiveEncounterViewModel, ActiveEncounterPage>();
        Configure<EncounterTabViewModel, EncounterTabPage>();
        Configure<AbilityCRUDViewModel, AbilityCRUDPage>();
        Configure<AbilityEditViewModel, AbilityEditPage>();
        //Configure<CreatureAbilityViewModel, CreatureAbilityPage>();
        Configure<CreatureDisplayTabViewModel, CreatureDisplayTabPage>();
        Configure<EncounterDamageTabViewModel, EncounterDamageTabPage>();
        Configure<LogTabViewModel, LogTabPage>();

        //experimental:
        Configure<EncounterAddCreaturesTabViewModel, EncounterAddCreaturesTabPage>();

    }

    public Type GetPageType(string key)
    {
        Type? pageType;
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    private void Configure<VM, V>()
        where VM : ObservableObject
        where V : Page
    {
        lock (_pages)
        {
            var key = typeof(VM).FullName!;
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(V);
            if (_pages.ContainsValue(type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);
        }
    }
}
