using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using Windows.Media.Playback;

namespace EasyEncounters.ViewModels;
public partial class RunSessionViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService _dataService;
    private readonly INavigationService _navigationService;
    private readonly IActiveEncounterService _activeEncounterService;


    public ObservableCollection<EncounterData> EncounterData { get; private set; } = new ObservableCollection<EncounterData>();

    public RunSessionViewModel(IDataService dataService, INavigationService navigationService, IActiveEncounterService encounterService)
    {
        _dataService = dataService;
        _activeEncounterService = encounterService;
        _navigationService = navigationService;
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
                EncounterData.Add(item);
            }

        }
    }

    [RelayCommand]
    private async void EncounterSelected(EncounterData parameter)
    {
        //if(parameter is EncounterData)
        //{
        if (parameter != null)
        {

            var active = await _activeEncounterService.CreateActiveEncounterAsync(parameter.Encounter, parameter.Party);
            _navigationService.NavigateTo(typeof(RunEncounterViewModel).FullName!, active);
        }
        //}
    }

    [RelayCommand]
    private void EncounterFilter(string text)
    {
        List<EncounterData> tempFiltered;
        var holding = new List<EncounterData>();

        tempFiltered = EncounterData.Where(x => x.Encounter.Name.Contains(text, StringComparison.InvariantCultureIgnoreCase)).ToList();

        for (int i = EncounterData.Count - 1; i >= 0; i--)
        {
            var item = EncounterData[i];
            if (!tempFiltered.Contains(item))
            {
                EncounterData.Remove(item);
                holding.Add(item);
            }
        }
        foreach (var encounter in holding)
        {
            EncounterData.Add(encounter);
        }
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
            tmp = EncounterData.OrderByDescending(x => x.DifficultyDescription, new DifficultyComparer()).ToList();
        else
            tmp = EncounterData.OrderBy(x => x.DifficultyDescription, new DifficultyComparer()).ToList();

        EncounterData.Clear();
        foreach (var item in tmp)
            EncounterData.Add(item);
    }

    //bit smelly and lazy, but no expectation this will ever be reused elsewhere.
    internal class DifficultyComparer : Comparer<string>
    {
        public override int Compare(string x, string y)
        {
            var indexX = GetIndex(x);
            var indexY = GetIndex(y);

            if (indexX == indexY)
                return 0;
            if (indexX > indexY)
                return 1;
            return -1;
        }

        private int GetIndex(string key)
        {
            switch (key)
            {
                case "Trivial":
                    return 0;
                case "Easy":
                    return 1;
                case "Medium":
                    return 2;
                case "Hard":
                    return 3;
                case "Deadly":
                    return 4;
                default:
                    return 5;
            }
        }
    }
}
