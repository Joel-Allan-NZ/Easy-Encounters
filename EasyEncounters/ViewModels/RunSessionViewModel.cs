using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Services.Filter;

namespace EasyEncounters.ViewModels;

public partial class RunSessionViewModel : ObservableRecipient, INavigationAware
{
    private readonly IActiveEncounterService _activeEncounterService;
    private readonly IDataService _dataService;
    private readonly IList<EncounterData> _encounters;
    private readonly IFilteringService _filteringService;
    private readonly INavigationService _navigationService;
    private readonly IEncounterService _encounterService;

    [ObservableProperty]
    private EncounterFilter _encounterFilterValues;

    public RunSessionViewModel(IDataService dataService, INavigationService navigationService, IActiveEncounterService activeEncounterService, IFilteringService filteringService,
        IEncounterService encounterService)
    {
        _encounterService = encounterService;
        _dataService = dataService;
        _activeEncounterService = activeEncounterService;
        _navigationService = navigationService;
        _filteringService = filteringService;
        _encounters = new List<EncounterData>();
        _encounterFilterValues = (EncounterFilter)_filteringService.GetFilterValues<EncounterData>();
    }

    public event EventHandler<DataGridColumnEventArgs>? Sorting;

    public ObservableCollection<EncounterData> EncounterData { get; private set; } = new ObservableCollection<EncounterData>();

    public void OnNavigatedFrom()
    {
    }

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is Party)
        {
            var party = (Party)parameter;
            EncounterData.Clear();

            var encounters = await _dataService.GetAllEncountersAsync();
            var data = _encounterService.GenerateEncounterData(party, encounters);

            //var data = await _dataService.GetAllEncounterDataAsync(party);
            foreach (var item in data)
            {
                _encounters.Add(item);
                EncounterData.Add(item);
            }
            EncounterFilterValues = (EncounterFilter)_filteringService.GetFilterValues<EncounterData>();
        }
    }

    protected virtual void OnSorting(DataGridColumnEventArgs e)
    {
        Sorting?.Invoke(this, e);
    }

    [RelayCommand]
    private void AddEncounter()
    {
        _navigationService.NavigateTo(typeof(EncounterEditViewModel).FullName!, null);
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

    [RelayCommand]
    private void EncounterFilter(string text)
    {
        var filtered = _filteringService.Filter(_encounters, EncounterFilterValues, text);
        EncounterData.Clear();
        foreach (var encounter in filtered)
            EncounterData.Add(encounter);
    }

    [RelayCommand]
    private async Task EncounterSelected(EncounterData parameter)
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
        var filtered = _filteringService.Filter(_encounters, EncounterFilterValues, text);

        if (String.IsNullOrEmpty(text))
        {
            EncounterData.Clear();
            foreach (var encounter in filtered)
                EncounterData.Add(encounter);
        }
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
}