using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using EasyEncounters.Services;
using Microsoft.UI.Dispatching;

namespace EasyEncounters.ViewModels;
public partial class EncounterEditViewModel : ObservableRecipient, INavigationAware
{
    private IList<CreatureViewModel> _creatureCache;

    public ObservableCollection<CreatureViewModel> EncounterCreatures
    {
        get; private set;
    } = new();

    public ObservableCollection<CreatureViewModel> Creatures
    {
        get; private set;
    } = new();

    [ObservableProperty]
    private Encounter _encounter;

    [ObservableProperty]
    private double _minimumCRFilter;

    [ObservableProperty]
    private double _maximumCRFilter;

    [ObservableProperty]
    private List<CreatureViewModel> searchSuggestions;

    [RelayCommand]
    private async void CommitChanges(object obj)
    {
        await _dataService.SaveAddAsync(Encounter);
        if (_navigationService.CanGoBack)
            _navigationService.GoBack();
    }

    [RelayCommand]
    private void AddCreature(object obj)
    {
        if (obj != null && obj is CreatureViewModel)
        {
            CreatureViewModel creature = (CreatureViewModel)obj;
            EncounterCreatures.Add(new CreatureViewModel(creature.Creature));
            //Encounter.Creatures.Add(creature.Creature);
            _encounterService.AddCreature(Encounter, creature.Creature);
        }
    }

    [RelayCommand]
    private void RemoveCreature(object obj)
    {
        if (obj != null && obj is CreatureViewModel)
        {
            CreatureViewModel toRemove = (CreatureViewModel)obj;
            EncounterCreatures.Remove(EncounterCreatures.First(x => x.Creature == toRemove.Creature));
            _encounterService.RemoveCreature(Encounter, toRemove.Creature);
            //Encounter.Creatures.Remove(toRemove.Creature);
        }
    }

    public event EventHandler<DataGridColumnEventArgs> Sorting;
    protected virtual void OnSorting(DataGridColumnEventArgs e)
    {
        EventHandler<DataGridColumnEventArgs> handler = Sorting;
        if (handler != null)
            handler(this, e);
    }

    [RelayCommand]
    private void SearchTextChange(string text)
    {
        if (String.IsNullOrEmpty(text))
        {
            var filtered = _filteringService.Filter(_creatureCache, x => x.Creature.LevelOrCR, MinimumCRFilter, MaximumCRFilter);
            Creatures.Clear();
            foreach (var creature in filtered)
                Creatures.Add(creature);
        }
        SearchSuggestions = _filteringService.Filter(Creatures, x => x.Creature.Name, text).ToList();
    }

    [RelayCommand]
    private void CreatureFilter(string text)
    {
        List<FilterCriteria<CreatureViewModel>> criteria = new List<FilterCriteria<CreatureViewModel>>()
        {
            new FilterCriteria<CreatureViewModel>(x => x.Creature.LevelOrCR, MinimumCRFilter, MaximumCRFilter),
            new FilterCriteria<CreatureViewModel>(x => x.Creature.Name, text)
        };

        var filtered = _filteringService.Filter(_creatureCache, criteria);
        Creatures.Clear();
        foreach (var creature in filtered)
            Creatures.Add(creature);
    }

    [RelayCommand]
    private void DataGridSort(DataGridColumnEventArgs e)
    {
        OnSorting(e);
        if (e.Column.Tag.ToString() == "CreatureName")
        {
            if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                SortCreaturesByName(false);
            else
                SortCreaturesByName(true);
        }
        else if (e.Column.Tag.ToString() == "CreatureCR")
        {
            if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                SortCreaturesByCR(false);
            else
                SortCreaturesByCR(true);
        }

        if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
        {
            e.Column.SortDirection = DataGridSortDirection.Ascending;
        }
        else
        {
            e.Column.SortDirection = DataGridSortDirection.Descending;
        }

    }

    private void SortCreaturesByName(bool ascending)
    {
        IEnumerable<CreatureViewModel> tmp;

        if (!ascending)
            tmp = Creatures.OrderBy(x => x.Creature.Name).ToList();
        else
            tmp = Creatures.OrderByDescending(x => x.Creature.Name).ToList();

        Creatures.Clear();
        foreach (var creature in tmp)
            Creatures.Add(creature);

    }

    private void SortCreaturesByCR(bool ascending)
    {
        IEnumerable<CreatureViewModel> tmp;

        if (!ascending)
            tmp = Creatures.OrderBy(x => x.Creature.LevelOrCR).ToList();
        else
            tmp = Creatures.OrderByDescending(x => x.Creature.LevelOrCR).ToList();

        Creatures.Clear();
        foreach (var creature in tmp)
            Creatures.Add(creature);
    }


    private readonly INavigationService _navigationService;
    private readonly IDataService _dataService;
    private readonly IEncounterService _encounterService;
    private readonly IFilteringService _filteringService;
    //private readonly DispatcherQueueTimer _filterTimer;

    public EncounterEditViewModel(INavigationService navigationService, IDataService dataService, IEncounterService encounterService, IFilteringService filteringService)
    {
        _navigationService = navigationService;
        _dataService = dataService;
        _encounterService = encounterService;

        //var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        //_filterTimer = dispatcherQueue.CreateTimer();

        WeakReferenceMessenger.Default.Register<CreatureDeleteRequestMessage>(this, (r, m) =>
        {
            RemoveCreature(m.Parameter);
        });

        WeakReferenceMessenger.Default.Register<CreatureCopyRequestMessage>(this, (r, m) =>
        {
            AddCreature(m.Parameter);
        });
        _filteringService = filteringService;
        _creatureCache = new List<CreatureViewModel>();
    }


    public void OnNavigatedFrom()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);

    }

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter != null && parameter is Encounter)
        {
            Encounter = (Encounter)parameter;

            EncounterCreatures.Clear();
            foreach (var creature in Encounter.Creatures)
                EncounterCreatures.Add(new CreatureViewModel(creature));
        }
        else
        {
            Encounter = new Encounter(); //create a new encounter if you aren't editing an existing one.

        }

        Creatures.Clear();
        foreach (var creature in await _dataService.GetAllCreaturesAsync())
            Creatures.Add(new CreatureViewModel(creature));

        MaximumCRFilter = 30; //that's as high as she goes cap'n
        _creatureCache = new List<CreatureViewModel>(Creatures);

        SearchSuggestions = new(Creatures);
    }
}
