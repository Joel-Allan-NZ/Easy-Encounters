using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.UI;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Messages;
using EasyEncounters.Models;
using EasyEncounters.Services.Filter;
using Microsoft.UI.Dispatching;

namespace EasyEncounters.ViewModels;

public partial class EncounterCRUDViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService _dataService;
    private readonly INavigationService _navigationService;
    private readonly IFilteringService _filteringService;
    private IList<ObservableEncounter>? _encounterCache;

    [ObservableProperty]
    private List<Campaign> _campaigns;

    [ObservableProperty]
    private ObservableEncounterFilter _encounterFilterValues;

    public EncounterCRUDViewModel(INavigationService navigationService, IDataService dataService, IFilteringService filteringService)
    {
        _dataService = dataService;
        _navigationService = navigationService;
        _filteringService = filteringService;

        _encounterFilterValues = (ObservableEncounterFilter)_filteringService.GetFilterValues<ObservableEncounter>();

        _campaigns = new();
    }

    public ObservableCollection<ObservableEncounter> Encounters
    {
        get; private set;
    } = new();

    public void OnNavigatedFrom()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public async void OnNavigatedTo(object parameter)
    {
        Encounters.Clear();
        foreach (var encounter in await _dataService.GetAllEncountersAsync())
        {
            Encounters.Add(new ObservableEncounter(encounter));
        }

        _encounterCache = new List<ObservableEncounter>(Encounters);

        Campaigns = new(await _dataService.GetAllCampaignsAsync());
    }

    [RelayCommand]
    private async Task AddEncounter()
    {
        ObservableEncounter observable = new(new());
        Encounters.Add(observable);
        _encounterCache?.Add(observable);
        await _dataService.SaveAddAsync(observable.Encounter);

        EditEncounter(observable.Encounter);
    }

    [RelayCommand]
    private async Task CopyEncounter(object parameter)
    {
        if (parameter != null && parameter is Encounter)
        {
            var copied = await _dataService.CopyAsync(parameter as Encounter);

            if (copied != null)
            {
                ObservableEncounter encounter = new(copied);
                Encounters.Add(encounter);
                _encounterCache?.Add(encounter);
            }
        }
    }

    [RelayCommand]
    private async Task DeleteEncounter(object parameter)
    {
        if (parameter != null && parameter is Encounter encounter)
        {
            var encounterToRemove = Encounters.First(x => x.Encounter == encounter);
            Encounters.Remove(encounterToRemove);
            _encounterCache?.Remove(encounterToRemove);
            await _dataService.DeleteAsync(encounter);
        }
    }

    [RelayCommand]
    private void EditEncounter(object parameter)
    {
        //if (parameter != null && parameter is Encounter)
        //{
        //    _navigationService.NavigateTo(typeof(EncounterEditNavigationViewModel).FullName!, parameter);
        //}
        //else
        if (parameter != null && parameter is ObservableEncounter model)
        {
            _navigationService.NavigateTo(typeof(EncounterEditNavigationViewModel).FullName!, model);
        }
    }

    [RelayCommand]
    private void DataGridSort(DataGridColumnEventArgs e)
    {
        EncounterFilterValues.SortCollection(Encounters, e);
    }

    [RelayCommand]
    private void SearchTextChange(string text)
    {
        if (_encounterCache == null)
            return;

        var filtered = _filteringService.Filter(_encounterCache, EncounterFilterValues, text);
        if (String.IsNullOrEmpty(text))
        {
            Encounters.Clear();
            foreach (var Encounter in filtered)
            {
                Encounters.Add(Encounter);
            }
        }
    }

    [RelayCommand]
    private void EncounterFilter(string text)
    {
        if (_encounterCache == null)
            return;

        var filtered = _filteringService.Filter(_encounterCache, EncounterFilterValues, text);
        Encounters.Clear();
        foreach (var encounter in filtered)
        {
            Encounters.Add(encounter);
        }
    }

    [RelayCommand]
    private void ClearFilters()
    {
        EncounterFilterValues.ResetFilter();
        EncounterFilter("");
    }

}