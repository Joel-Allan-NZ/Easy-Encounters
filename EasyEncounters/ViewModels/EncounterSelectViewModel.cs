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

namespace EasyEncounters.ViewModels;

public partial class EncounterSelectViewModel : ObservableRecipient, INavigationAware
{
    private readonly IActiveEncounterService _activeEncounterService;
    private readonly IDataService _dataService;
    private IList<ObservableEncounter> _encounterCache;
    private readonly IFilteringService _filteringService;
    private readonly INavigationService _navigationService;
    private readonly IEncounterService _encounterService;

    [ObservableProperty]
    private Party _party;

    [ObservableProperty]
    private ObservableEncounterFilter _encounterFilterValues;


    public EncounterSelectViewModel(IDataService dataService, INavigationService navigationService, IActiveEncounterService activeEncounterService, IFilteringService filteringService,
        IEncounterService encounterService)
    {
        _encounterService = encounterService;
        _dataService = dataService;
        _activeEncounterService = activeEncounterService;
        _navigationService = navigationService;
        _filteringService = filteringService;
        _encounterCache = new List<ObservableEncounter>();
        _encounterFilterValues = (ObservableEncounterFilter)_filteringService.GetFilterValues<ObservableEncounter>();
    }

    public event EventHandler<DataGridColumnEventArgs>? Sorting;

    public ObservableCollection<ObservableEncounter> Encounters { get; private set; } = new ObservableCollection<ObservableEncounter>();

    public void OnNavigatedFrom()
    {
    }

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is Party party)
        {
            Party = party;
            Encounters.Clear();

            var encounters = await _dataService.GetCampaignEncountersAsync(Party.Campaign, true);

            //var data = _encounterService.GenerateEncounterData(party, encounters);
            foreach(var encounter in encounters)
            {
                ObservableEncounter obs = new(encounter)
                {
                    EncounterDifficulty = _encounterService.DetermineDifficultyForParty(encounter, Party)
                };

                Encounters.Add(obs);
            }

            _encounterCache = new List<ObservableEncounter>(Encounters);

            EncounterFilterValues = (ObservableEncounterFilter)_filteringService.GetFilterValues<ObservableEncounter>();
        }
    }

    [RelayCommand]
    private void AddEncounter()
    {
        _navigationService.NavigateTo(typeof(EncounterEditNavigationViewModel).FullName!, null);
    }


    [RelayCommand]
    private void DataGridSort(DataGridColumnEventArgs e)
    {
        EncounterFilterValues.SortCollection(Encounters, e);
    }

    [RelayCommand]
    private void EncounterFilter(string text)
    {
        var filtered = _filteringService.Filter(_encounterCache, EncounterFilterValues, text);
        Encounters.Clear();
        foreach (var encounter in filtered)
            Encounters.Add(encounter);
    }

    [RelayCommand]
    private async Task SelectEncounter(ObservableEncounter parameter)
    {
        if (parameter != null)
        {
            var active = await _activeEncounterService.CreateActiveEncounterAsync(parameter.Encounter, Party);
            _navigationService.NavigateTo(typeof(EncounterTabViewModel).FullName!, active, true);
        }
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
    private void ClearFilters()
    {
        EncounterFilterValues.ResetFilter();
        EncounterFilter("");
    }
}