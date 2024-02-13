﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Services;
using EasyEncounters.Messages;
using EasyEncounters.Models;
using EasyEncounters.Services;

namespace EasyEncounters.ViewModels;
public partial class EncounterAddCreaturesTabViewModel : ObservableRecipientTab
{
    private readonly IDataService _dataService;
    private readonly IFilteringService _filteringService;

    private IList<CreatureViewModel> _creatureCache;

    /// <summary>
    /// Additional creatures to add to the active encounter, and their quantities
    /// </summary>
    public ObservableCollection<ObservableKVP<CreatureViewModel, int>> EncounterCreaturesByCount
    {
        get; private set;
    } = new();

    public ObservableCollection<CreatureViewModel> Creatures
    {
        get; private set;
    } = new();

    private ActiveEncounter? _activeEncounter;

    [ObservableProperty]
    private double _minimumCRFilter;

    [ObservableProperty]
    private double _maximumCRFilter;

    [ObservableProperty]
    private List<CreatureViewModel> searchSuggestions;

    [RelayCommand]
    private async void CommitChanges(object obj)
    {
        //foreach(var kvp in EncounterCreaturesByCount)
        //{
        //    for(int i =0; i<kvp.Value; i++)
        //    {
        //        _activeEncounterService.AddCreatureInProgress(_activeEncounter, kvp.Key.Creature);
        //    }
        //}

        WeakReferenceMessenger.Default.Send(new AddCreaturesRequestMessage(EncounterCreaturesByCount));

        //todo: send message to encounter tab, ensuring list of vms repopulated.
        //todo: close tab when finished.
        //NB: doesn't set their initiative currently, but you can drag-and-drop it in the active encounter


    }

    public EncounterAddCreaturesTabViewModel(IDataService dataService, IFilteringService filteringService)
    {
        _filteringService = filteringService;
        _dataService = dataService;
        _activeEncounter = null;

    }

    public override void OnTabClosed()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public async override void OnTabOpened(object parameter)
    {
        if(parameter != null && parameter is ActiveEncounter)
        {
            _activeEncounter = (ActiveEncounter)parameter;
        }

        Creatures.Clear();
        foreach (var creature in await _dataService.GetAllCreaturesAsync())
            Creatures.Add(new CreatureViewModel(creature));

        MaximumCRFilter = 30; //that's as high as she goes cap'n
        _creatureCache = new List<CreatureViewModel>(Creatures);

        SearchSuggestions = new(Creatures);
    }

    [RelayCommand]
    private void AddCreature(object obj)
    {
        if (obj != null && obj is CreatureViewModel)
        {
            CreatureViewModel creature = (CreatureViewModel)obj;

            var match = EncounterCreaturesByCount.FirstOrDefault(x => x.Key.Creature.Equals(creature.Creature));

            if (match == null)
            {
                EncounterCreaturesByCount.Add(new ObservableKVP<CreatureViewModel, int>(creature, 1));
            }
            else
                match.Value++;

            //EncounterCreatures.Add(new CreatureViewModel(creature.Creature));
            //Encounter.Creatures.Add(creature.Creature);
            //_encounterService.AddCreature(Encounter, creature.Creature); //todo: switch this to dictionary?
        }
    }

    [RelayCommand]
    private void RemoveCreature(object obj)
    {
        if (obj != null && obj is CreatureViewModel)
        {
            CreatureViewModel toRemove = (CreatureViewModel)obj;

            var match = EncounterCreaturesByCount.FirstOrDefault(x => x.Key.Creature.Equals(toRemove.Creature));
            EncounterCreaturesByCount.Remove(match);

            //EncounterCreatures.Remove(EncounterCreatures.First(x => x.Creature == toRemove.Creature));
            //while (Encounter.Creatures.Contains(toRemove.Creature))
            //    _encounterService.RemoveCreature(Encounter, toRemove.Creature);
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


}
