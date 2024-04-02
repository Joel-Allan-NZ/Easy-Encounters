using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Models;
using EasyEncounters.Services.Filter;
using Microsoft.EntityFrameworkCore;
using Windows.Services.Maps;

namespace EasyEncounters.ViewModels;

public partial class EncounterSelectViewModel : ObservableRecipient, INavigationAware
{
    private readonly IActiveEncounterService _activeEncounterService;
    private readonly IDataService _dataService;
    private readonly IFilteringService _filteringService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private Party _party;

    [ObservableProperty]
    private EncounterFilter _encounterFilterValues;

    public List<Campaign> Campaigns
    {
    get; set; }


    public EncounterSelectViewModel(IDataService dataService, INavigationService navigationService, IActiveEncounterService activeEncounterService, IFilteringService filteringService)
    {
        _dataService = dataService;
        _activeEncounterService = activeEncounterService;
        _navigationService = navigationService;
        _filteringService = filteringService;
        _encounterFilterValues = (EncounterFilter)_filteringService.GetFilterValues<Encounter>();
        Campaigns = new();
        

    }

    public void OnNavigatedFrom()
    {
    }

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is Party party)
        {
            Campaigns = new();
            Party = party;

            Campaigns.Add(Party.Campaign);

            EncounterFilterValues = (EncounterFilter)_filteringService.GetFilterValues<Encounter>();
            await EncounterFilterValues.ResetAsync();
        }
    }

    [RelayCommand]
    private void AddEncounter()
    {
        _navigationService.NavigateTo(typeof(EncounterEditNavigationViewModel).FullName!, null);
    }

    [RelayCommand]
    private async Task SelectEncounter(ObservableEncounter parameter)
    {
        if (parameter != null)
        {
            var encounter = _dataService.Encounters().Where(x => x.Id == parameter.Encounter.Id).Include(x => x.CreaturesByCount).Include(x => x.Creatures).ThenInclude(x => x.Abilities).First();
            var active = await _activeEncounterService.CreateActiveEncounterAsync(encounter, Party);
            _navigationService.NavigateTo(typeof(EncounterTabViewModel).FullName!, active, true);
        }
    }
}