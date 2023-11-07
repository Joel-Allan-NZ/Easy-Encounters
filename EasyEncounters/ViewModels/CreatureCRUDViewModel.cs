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
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.Search.Core;
using Windows.Services.Maps;

namespace EasyEncounters.ViewModels;
public partial class CreatureCRUDViewModel : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IDataService _dataService;
    private readonly IFilteringService _filteringService;

    private IList<CreatureViewModel> _creatureCache;

    public ObservableCollection<CreatureViewModel> Creatures
    {
        get; private set;
    } = new();

    [ObservableProperty]
    private double _minimumCRFilter;

    [ObservableProperty]
    private double _maximumCRFilter;

    [ObservableProperty]
    private List<CreatureViewModel> searchSuggestions;

    //private DispatcherQueueTimer _filterTimer;

    public CreatureCRUDViewModel(IDataService dataService, INavigationService navigationService, IFilteringService filteringService)
    {
        _dataService = dataService;
        _navigationService = navigationService;
        _filteringService = filteringService;

        WeakReferenceMessenger.Default.Register<CreatureCopyRequestMessage>(this, (r, m) =>
        {
            CopyCreature(m.Parameter.Creature);
        });
        WeakReferenceMessenger.Default.Register<CreatureDeleteRequestMessage>(this, (r, m) =>
        {
            DeleteCreature(m.Parameter.Creature);
        });
        WeakReferenceMessenger.Default.Register<CreatureEditRequestMessage>(this, (r, m) =>
        {
            EditCreature(m.Parameter);
        });

        //var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        //_filterTimer = dispatcherQueue.CreateTimer();



    }

    [RelayCommand]
    private void EditCreature(object parameter)
    {
        if(parameter is CreatureViewModel)
        {
            //todo: pass a copy of the creature rather than the original, so changes are discarded if user hits back button rather than committing changes.
            _navigationService.NavigateTo(typeof(CreatureEditViewModel).FullName!, ((CreatureViewModel)parameter).Creature);
        }
    }

    [RelayCommand]
    private async void AddNewCreature()
    {
        var creature = new Creature();
        var vm = new CreatureViewModel(creature);
        Creatures.Add(vm);
        _creatureCache.Add(vm);
        await _dataService.SaveAddAsync(creature);
        _navigationService.NavigateTo(typeof(CreatureEditViewModel).FullName!, creature);
    }

    [RelayCommand]
    private async void DeleteCreature(object parameter)
    {
        if (parameter != null && parameter is Creature)
        {
            var creature = (Creature)parameter;
            var creatureVM = Creatures.First(x => x.Creature == creature);
            Creatures.Remove(creatureVM);
            _creatureCache.Remove(creatureVM);
            await _dataService.DeleteAsync(creature);
        }
    }

    [RelayCommand]
    private async void CopyCreature(object parameter)
    {
        if(parameter != null && parameter is Creature)
        {
            var copied = await _dataService.CopyAsync(parameter as Creature);
            if(copied != null)
            {
                var creature = new CreatureViewModel(copied);
                Creatures.Add(creature);
                _creatureCache.Add(creature);
            }

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

    public void OnNavigatedFrom()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
    public async void OnNavigatedTo(object parameter)
    {
        Creatures.Clear();
        foreach (var creature in await _dataService.GetAllCreaturesAsync())
            Creatures.Add(new CreatureViewModel(creature));

        _creatureCache = new List<CreatureViewModel>(Creatures);
        SearchSuggestions = new(Creatures);
        MaximumCRFilter = 30;
        

    }
}
