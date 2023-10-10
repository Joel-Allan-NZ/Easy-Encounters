using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABI.System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Attributes;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Services;
using Microsoft.UI.Xaml.Controls;
using Windows.Media.Playback;
using Windows.Security.ExchangeActiveSyncProvisioning;

namespace EasyEncounters.ViewModels;
public partial class RunSessionViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService _dataService;
    private readonly INavigationService _navigationService;
    private readonly IActiveEncounterService _activeEncounterService;
    private readonly IFilteringService _filteringService;
    private readonly IList<EncounterDifficulty> _difficulties = Enum.GetValues(typeof(EncounterDifficulty)).Cast<EncounterDifficulty>().ToList();

    private IList<EncounterData> _encounters;

    //private Dictionary<string, FilterCriteria<EncounterData>> _filterCriteria;

    public IList<EncounterDifficulty> Difficulties => _difficulties;


    public ObservableCollection<EncounterData> EncounterData { get; private set; } = new ObservableCollection<EncounterData>();

    [ObservableProperty]
    private List<EncounterData> searchSuggestions;

    [ObservableProperty]
    private EncounterDifficulty _minimumDifficulty;

    [ObservableProperty]
    private EncounterDifficulty _maximumDifficulty;

    [ObservableProperty]
    private int _minimumEnemiesFilter;

    [ObservableProperty]
    private int _maximumEnemiesFilter;

    public RunSessionViewModel(IDataService dataService, INavigationService navigationService, IActiveEncounterService encounterService, IFilteringService filteringService)
    {
        _dataService = dataService;
        _activeEncounterService = encounterService;
        _navigationService = navigationService;
        _filteringService = filteringService;
        //_filterCriteria = new Dictionary<string, FilterCriteria<EncounterData>>();
        _encounters = new List<EncounterData>();
    }

    public void OnNavigatedFrom()
    {

    }
    public async void OnNavigatedTo(object parameter)
    {

        if (parameter is Party)
        {
            
            var party = (Party) parameter;
            EncounterData.Clear();

            var data = await _dataService.GetAllEncounterDataAsync(party);
            foreach (var item in data)
            {
                _encounters.Add(item);
                EncounterData.Add(item);
            }
            MaximumEnemiesFilter = 1000;
            MaximumDifficulty = EncounterDifficulty.VeryDeadly;
            SearchSuggestions = new List<EncounterData>(EncounterData);
        }
        
    }

    [RelayCommand]
    private async void EncounterSelected(EncounterData parameter)
    {

        if (parameter != null)
        {

            var active = await _activeEncounterService.CreateActiveEncounterAsync(parameter.Encounter, parameter.Party);
            _navigationService.NavigateTo(typeof(EncounterTabViewModel).FullName!, active);
        }

    }

    [RelayCommand]
    private void SearchTextChange(string text)
    {
        if (text == "")
        {
            List<FilterCriteria<EncounterData>> criteriaList = new List<FilterCriteria<EncounterData>>()
            {
                new FilterCriteria<EncounterData>(x => x.DifficultyDescription, MinimumDifficulty, MaximumDifficulty),
                new FilterCriteria<EncounterData>(x => x.Encounter.Creatures.Count, MinimumEnemiesFilter, MaximumEnemiesFilter)
            };
            var filtered = _filteringService.Filter(_encounters, criteriaList);
            EncounterData.Clear();
            foreach (var encounter in filtered)
                EncounterData.Add(encounter);

        }
        SearchSuggestions = _filteringService.Filter(EncounterData, new FilterCriteria<EncounterData>(x => x.Encounter.Name, text)).ToList();
    }

    [RelayCommand]
    private void EncounterFilter(string text)
    {
        //conscious choice not to add the text to current filtering rules, but worth observing in future to see
        //if it makes more sense to do so and then clear the filter when text changes
        List<FilterCriteria<EncounterData>> criteriaList = new List<FilterCriteria<EncounterData>>()
        {
            new FilterCriteria<EncounterData>(x => x.Encounter.Name, text),
            new FilterCriteria<EncounterData>(x => x.DifficultyDescription, MinimumDifficulty, MaximumDifficulty),
            new FilterCriteria<EncounterData>(x => x.Encounter.Creatures.Count, MinimumEnemiesFilter, MaximumEnemiesFilter)
        };
        var filtered = _filteringService.Filter(_encounters, criteriaList);
        EncounterData.Clear();
        foreach (var encounter in filtered)
            EncounterData.Add(encounter);
    

    }

    [RelayCommand]
    private void AddEncounter()
    {
        _navigationService.NavigateTo(typeof(EncounterEditViewModel).FullName!, null);
    }


    public event EventHandler<DataGridColumnEventArgs> Sorting;

    protected virtual void OnSorting(DataGridColumnEventArgs e)
    {
        EventHandler<DataGridColumnEventArgs> handler = Sorting;
        if(handler!=null)
            handler(this, e);
    }

    [RelayCommand]
    private void DataGridSort(DataGridColumnEventArgs e)
    {
        OnSorting(e);

        if (e.Column.Tag.ToString() == "EncounterName")
        {
            if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
            {
                SortEncountersByName(false);
                e.Column.SortDirection = DataGridSortDirection.Ascending;
            }
            else
            {
                SortEncountersByName(true);
                e.Column.SortDirection = DataGridSortDirection.Descending;
            }
        }
        else if (e.Column.Tag.ToString() == "EnemyCount")
        {
            if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
            {
                SortEncountersByEnemyCount(false);
                e.Column.SortDirection = DataGridSortDirection.Ascending;
            }
            else
            {
                SortEncountersByEnemyCount(true);
                e.Column.SortDirection = DataGridSortDirection.Descending;
            }
        }
        else if (e.Column.Tag.ToString() == "Difficulty")
        {
            if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
            {
                SortEncountersByDifficulty(false);
                e.Column.SortDirection = DataGridSortDirection.Ascending;
            }
            else
            {
                SortEncountersByDifficulty(true);
                e.Column.SortDirection = DataGridSortDirection.Descending;
            }
        }

    }

    private void SortEncountersByName(bool ascending)
    {
        IEnumerable<EncounterData> tmp;
        if (!ascending)
        {
            tmp = EncounterData.OrderBy(x => x.Encounter.Name).ToList();
        }
        else
        {
            tmp = EncounterData.OrderByDescending(x => x.Encounter.Name).ToList();
        }
        EncounterData.Clear();
        foreach (var item in tmp)
            EncounterData.Add(item);
    }

    private void SortEncountersByEnemyCount(bool ascending)
    {
        IEnumerable<EncounterData> tmp;
        if (ascending)
            tmp = EncounterData.OrderByDescending(x => x.Encounter.Creatures.Count).ToList();
        else
            tmp = EncounterData.OrderBy(x => x.Encounter.Creatures.Count).ToList();

        EncounterData.Clear();
        foreach (var item in tmp)
            EncounterData.Add(item);
    }

    private void SortEncountersByDifficulty(bool ascending)
    {
        IEnumerable<EncounterData> tmp;
        if (ascending)
            tmp = EncounterData.OrderByDescending(x => x.DifficultyDescription).ToList();
        else
            tmp = EncounterData.OrderBy(x => x.DifficultyDescription).ToList();

        EncounterData.Clear();
        foreach (var item in tmp)
            EncounterData.Add(item);
    }
}
