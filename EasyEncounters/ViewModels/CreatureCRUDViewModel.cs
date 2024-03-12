﻿using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Messages;
using EasyEncounters.Services.Filter;

namespace EasyEncounters.ViewModels;

public partial class CreatureCRUDViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService _dataService;
    private readonly IFilteringService _filteringService;
    private readonly INavigationService _navigationService;
    private IList<CreatureViewModel>? _creatureCache;
    private readonly ICreatureService _creatureService;

    [ObservableProperty]
    private CreatureFilter _creatureFilterValues;

    public CreatureCRUDViewModel(IDataService dataService, INavigationService navigationService, IFilteringService filteringService, ICreatureService creatureService)
    {
        _dataService = dataService;
        _navigationService = navigationService;
        _filteringService = filteringService;
        _creatureService = creatureService;

        _creatureFilterValues = (CreatureFilter)_filteringService.GetFilterValues<CreatureViewModel>();
    }

    public ObservableCollection<CreatureViewModel> Creatures
    {
        get; private set;
    } = new();

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

    }

    [RelayCommand]
    private async Task AddNewCreature()
    {
        var creature = _creatureService.Create();
        var vm = new CreatureViewModel(creature);
        Creatures.Add(vm);
        _creatureCache?.Add(vm);
        await _dataService.SaveAddAsync(creature);
        _navigationService.NavigateTo(typeof(CreatureEditNavigationPageViewModel).FullName!, creature);
    }

    [RelayCommand]
    private async Task CopyCreature(object parameter)
    {
        if (parameter != null && parameter is Creature)
        {
            var copied = await _dataService.CopyAsync(parameter as Creature);
            if (copied != null)
            {
                var creature = new CreatureViewModel(copied);
                Creatures.Add(creature);
                _creatureCache?.Add(creature);
            }
        }
    }

    [RelayCommand]
    private void DataGridSort(DataGridColumnEventArgs e)
    {
        CreatureFilterValues.SortCollection(Creatures, e);
    }

    [RelayCommand]
    private async Task DeleteCreature(object parameter)
    {
        if (parameter != null && parameter is Creature)
        {
            var creature = (Creature)parameter;
            var creatureVM = Creatures.First(x => x.Creature == creature);
            Creatures.Remove(creatureVM);
            _creatureCache?.Remove(creatureVM);
            await _dataService.DeleteAsync(creature);
        }
    }

    //private DispatcherQueueTimer _filterTimer;
    [RelayCommand]
    private void EditCreature(object parameter)
    {
        if (parameter is CreatureViewModel)
        {
            //todo: pass a copy of the creature rather than the original, so changes are discarded if user hits back button rather than committing changes.
            //_navigationService.NavigateTo(typeof(CreatureEditViewModel).FullName!, ((CreatureViewModel)parameter).Creature);

            //experiemental:
            _navigationService.NavigateTo(typeof(CreatureEditNavigationPageViewModel).FullName!, ((CreatureViewModel)parameter).Creature);
        }
    }

    [RelayCommand]
    private void SearchTextChange(string text)
    {
        if (_creatureCache == null)
            return;

        var filtered = _filteringService.Filter(_creatureCache, CreatureFilterValues, text);
        if (String.IsNullOrEmpty(text))
        {
            Creatures.Clear();
            foreach (var creature in filtered)
            {
                Creatures.Add(creature);
            }
        }
    }

    [RelayCommand]
    private void CreatureFilter(string text)
    {
        if (_creatureCache == null)
            return;

        var filtered = _filteringService.Filter(_creatureCache, CreatureFilterValues, text);
        Creatures.Clear();
        foreach (var creature in filtered)
        {
            Creatures.Add(creature);
        }
    }

    [RelayCommand]
    private void ClearFilters()
    {
        CreatureFilterValues.ResetFilter();
        CreatureFilter("");
    }
}