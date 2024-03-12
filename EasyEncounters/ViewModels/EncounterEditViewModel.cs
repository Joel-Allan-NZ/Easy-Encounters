using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Messages;
using EasyEncounters.Models;
using EasyEncounters.Services.Filter;

namespace EasyEncounters.ViewModels;

public partial class EncounterEditViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService _dataService;

    private readonly IEncounterService _encounterService;

    private readonly IFilteringService _filteringService;

    private readonly INavigationService _navigationService;

    private IList<CreatureViewModel> _creatureCache;

    [ObservableProperty]
    private CreatureFilter _creatureFilterValues;

    [ObservableProperty]
    private Encounter? _encounter;

    [ObservableProperty]
    private EncounterDifficulty _expectedDifficulty;

    [ObservableProperty]
    private Party? _selectedParty;

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
        _creatureFilterValues = (CreatureFilter)_filteringService.GetFilterValues<CreatureViewModel>();
    }

    public event EventHandler<DataGridColumnEventArgs>? Sorting;

    public ObservableCollection<CreatureViewModel> Creatures
    {
        get; private set;
    } = new();

    public ObservableCollection<ObservableKVP<CreatureViewModel, int>> EncounterCreaturesByCount
    {
        get; private set;
    } = new();

    public ObservableCollection<Party> Parties
    {
        get;
        private set;
    } = new();

    //private readonly DispatcherQueueTimer _filterTimer;
    public void OnNavigatedFrom()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter != null && parameter is Encounter)
        {
            Encounter = (Encounter)parameter;

            EncounterCreaturesByCount.Clear();
            foreach (var creature in Encounter.Creatures)
            {
                var match = EncounterCreaturesByCount.FirstOrDefault(x => x.Key.Creature.Equals(creature));

                if (match != null)
                    match.Value++;
                else
                    EncounterCreaturesByCount.Add(new ObservableKVP<CreatureViewModel, int>(new CreatureViewModel(creature), 1));
            }
        }
        else
        {
            Encounter = new Encounter(); //create a new encounter if you aren't editing an existing one.
        }

        Creatures.Clear();
        foreach (var creature in await _dataService.GetAllCreaturesAsync())
            Creatures.Add(new CreatureViewModel(creature));

        _creatureCache = new List<CreatureViewModel>(Creatures);

        foreach (var party in await _dataService.GetAllPartiesAsync())
            Parties.Add(party);

        ExpectedDifficulty = EncounterDifficulty.None;
        CreatureFilterValues = (CreatureFilter)_filteringService.GetFilterValues<CreatureViewModel>();
        CreatureFilterValues.ResetFilter();
        Creatures.CollectionChanged += PartyOrCreaturesChanged;

        
    }

    //protected virtual void OnSorting(DataGridColumnEventArgs e)
    //{
    //    Sorting?.Invoke(this, e);
    //}

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

            _encounterService.AddCreature(Encounter, creature.Creature); //todo: switch this to dictionary?
        }
    }

    [RelayCommand]
    private async Task CommitChanges(object obj)
    {
        if (Encounter != null)
        //update encounter creature counts to match the kvp version
        {
            Encounter.Creatures.Clear();
            foreach (var kvp in EncounterCreaturesByCount)
            {
                for (var i = 0; i < kvp.Value; i++)
                    Encounter.Creatures.Add(kvp.Key.Creature);
            }

            await _dataService.SaveAddAsync(Encounter);
            if (_navigationService.CanGoBack)
                _navigationService.GoBack();
        }
    }

    [RelayCommand]
    private void CreatureFilter(string text)
    {
        var filtered = _filteringService.Filter(_creatureCache, CreatureFilterValues, text);
        Creatures.Clear();
        foreach (var creature in filtered)
            Creatures.Add(creature);
    }

    [RelayCommand]
    private void DataGridSort(DataGridColumnEventArgs e)
    {
        CreatureFilterValues.SortCollection(Creatures, e);
    }

    partial void OnSelectedPartyChanged(Party? value) => SetDifficulty();

    private void PartyOrCreaturesChanged(object? sender, EventArgs e)
    {
        SetDifficulty();
    }

    [RelayCommand]
    private void RemoveCreature(object obj)
    {
        if (obj != null && obj is CreatureViewModel && Encounter != null)
        {
            CreatureViewModel toRemove = (CreatureViewModel)obj;

            var match = EncounterCreaturesByCount.FirstOrDefault(x => x.Key.Creature.Equals(toRemove.Creature));
            if (match != null)
                EncounterCreaturesByCount.Remove(match);

            while (Encounter.Creatures.Contains(toRemove.Creature))
                _encounterService.RemoveCreature(Encounter, toRemove.Creature);
        }
    }

    [RelayCommand]
    private void SearchTextChange(string text)
    {
        var filtered = _filteringService.Filter(_creatureCache, CreatureFilterValues, text);
        if (String.IsNullOrEmpty(text))
        {
            Creatures.Clear();
            foreach (var creature in filtered)
                Creatures.Add(creature);
        }
    }

    private void SetDifficulty()
    {
        if (SelectedParty == null || SelectedParty.Members.Count == 0 || Creatures.Count == 0)
            ExpectedDifficulty = EncounterDifficulty.None;
        else
        {
            ExpectedDifficulty = _encounterService.DetermineDifficultyForParty(Encounter, SelectedParty); //todo: possible issue here where the number of creatures doesn't reflect the count. TBD.
        }
    }

    [RelayCommand]
    private void ClearCreatureFilter()
    {
        CreatureFilterValues.ResetFilter();
        CreatureFilter("");
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

}