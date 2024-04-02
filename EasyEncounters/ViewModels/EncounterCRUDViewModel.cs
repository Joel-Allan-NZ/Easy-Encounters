using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Services;
using EasyEncounters.Models;
using EasyEncounters.Services.Filter;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.CustomAttributes;


namespace EasyEncounters.ViewModels;

public partial class EncounterCRUDViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService _dataService;
    private readonly INavigationService _navigationService;
    private readonly IFilteringService _filteringService;

    [ObservableProperty]
    private List<Campaign> _campaigns;

    [ObservableProperty]
    private EncounterFilter _encounterFilterValues;

    public EncounterCRUDViewModel(INavigationService navigationService, IDataService dataService, IFilteringService filteringService)
    {
        _dataService = dataService;
        _navigationService = navigationService;
        _filteringService = filteringService;

        _encounterFilterValues = (EncounterFilter)_filteringService.GetFilterValues<Encounter>();

        _campaigns = new();
    }



    public void OnNavigatedFrom()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public async void OnNavigatedTo(object parameter)
    {
        await EncounterFilterValues.ResetAsync();

        Campaigns = new(await _dataService.GetAllCampaignsAsync());
    }

    [RelayCommand]
    private async Task AddEncounter()
    {
        //ObservableEncounter observable = new(new());
        var enc = new Encounter("New Encounter");
        await _dataService.SaveAddAsync(enc);

        EditEncounter(new ObservableEncounter(enc));
    }

    [RelayCommand]
    private async Task CopyEncounter(object parameter)
    {
        if (parameter != null && parameter is ObservableEncounter encounter)
        {
            await _dataService.CopyAsync(encounter.Encounter);
            await EncounterFilterValues.RefreshAsync();

        }
    }

    [RelayCommand]
    private async Task DeleteEncounter(object parameter)
    {
        if (parameter != null && parameter is ObservableEncounter encounter)
        {
            var match = await _dataService.Encounters().FirstOrDefaultAsync(x => x.Id == encounter.Encounter.Id);
            if (match != null)
            {
                await _dataService.DeleteAsync(match);
                await EncounterFilterValues.RefreshAsync();
            }
        }
    }

    [RelayCommand]
    private void EditEncounter(object parameter)
    {
        if (parameter != null && parameter is ObservableEncounter model)
        {
            var toEdit = _dataService.Encounters().Where(x => x.Id == model.Encounter.Id).Include(x => x.Creatures).Include(x => x.CreaturesByCount).ToList().First();
            _navigationService.NavigateTo(typeof(EncounterEditNavigationViewModel).FullName!, new ObservableEncounter(toEdit));
        }
    }
}