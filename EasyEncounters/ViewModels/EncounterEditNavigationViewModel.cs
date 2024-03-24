using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABI.Windows.UI.Xaml.Interop;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Models;
using EasyEncounters.Services.Filter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EasyEncounters.ViewModels;

public partial class EncounterEditNavigationViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService _dataService;

    private readonly IEncounterService _encounterService;

    private readonly IFilteringService _filteringService;

    private readonly INavigationService _navigationService;

    private IList<ObservableCreature> _creatureCache;

    [ObservableProperty]
    private double _encounterCreatureCount;

    [ObservableProperty]
    private CreatureFilter _creatureFilterValues;

    [ObservableProperty]
    private ObservableEncounter? _encounter;

    [ObservableProperty]
    private EncounterDifficulty _expectedDifficulty;

    [ObservableProperty]
    private Party? _selectedParty;

    public ObservableCollection<Campaign> Campaigns
    {
        get; set;
    } = new();

    public EncounterEditNavigationViewModel(INavigationService navigationService, IDataService dataService, IEncounterService encounterService, IFilteringService filteringService)
    {
        _navigationService = navigationService;
        _dataService = dataService;
        _encounterService = encounterService;

        _filteringService = filteringService;
        _creatureCache = new List<ObservableCreature>();
        _creatureFilterValues = (CreatureFilter)_filteringService.GetFilterValues<ObservableCreature>();
    }

    public ObservableCollection<ObservableCreature> Creatures
    {
        get; private set;
    } = new();

    public ObservableCollection<ObservableKVP<ObservableCreature, int>> EncounterCreaturesByCount
    {
        get; private set;
    } = new();

    public ObservableCollection<Party> Parties
    {
        get;
        private set;
    } = new();

    public void OnNavigatedFrom()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    void OnKVPCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (var newitem in e.NewItems)
            {
                if (newitem is INotifyPropertyChanged notify)
                {
                    notify.PropertyChanged += OnKVPChanged;
                }

            }
        }
        RecountEncounterCreatures();
    }

    void OnKVPChanged(object? sender, PropertyChangedEventArgs e)
    {
        RecountEncounterCreatures();
    }

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter != null && parameter is ObservableEncounter)
        {
            Encounter = (ObservableEncounter)parameter;

            EncounterCreaturesByCount.Clear();          
            foreach (var creature in Encounter.Encounter.Creatures)
            {
                var match = EncounterCreaturesByCount.FirstOrDefault(x => x.Key.Creature.Equals(creature));

                if (match != null)
                {

                    match.Value++;
                }
                else
                {
                    
                    EncounterCreaturesByCount.Add(new(new ObservableCreature(creature), 1));
                }
            }
            foreach(var kvp in EncounterCreaturesByCount)
            {
                kvp.PropertyChanged += OnKVPChanged;
            }
            EncounterCreaturesByCount.CollectionChanged += OnKVPCollectionChanged;
            RecountEncounterCreatures();
        }
        else
        {
            Encounter = new ObservableEncounter(new()); //create a new encounter if you aren't editing an existing one.
        }

        Creatures.Clear();
        foreach (var creature in await _dataService.GetAllCreaturesAsync())
            Creatures.Add(new ObservableCreature((Creature)creature));

        _creatureCache = new List<ObservableCreature>(Creatures);

        foreach (var party in await _dataService.GetAllPartiesAsync())
            Parties.Add(party);

        ExpectedDifficulty = EncounterDifficulty.None;
        CreatureFilterValues = (CreatureFilter)_filteringService.GetFilterValues<ObservableCreature>();
        CreatureFilterValues.ResetFilter();
        //Creatures.CollectionChanged += PartyOrCreaturesChanged;

        var campaigns = await _dataService.GetAllCampaignsAsync();

        foreach(var campaign in campaigns)
        {
            Campaigns.Add(campaign);
        }


    }

    [RelayCommand]
    private void AddCreature(object obj)
    {
        if (obj != null && obj is ObservableCreature)
        {
            ObservableCreature creature = (ObservableCreature)obj;

            var match = EncounterCreaturesByCount.FirstOrDefault(x => x.Key.Creature.Equals(creature.Creature));

            if (match == null)
            {
                EncounterCreaturesByCount.Add(new ObservableKVP<ObservableCreature, int>(creature, 1));
            }
            else
                match.Value++;

            _encounterService.AddCreature(Encounter.Encounter, creature.Creature); //todo: switch this to dictionary?
        }
    }

    [RelayCommand]
    private async Task CommitChanges(object obj)
    {
        if (Encounter != null)
        //update encounter creature counts to match the kvp version
        {
            Encounter.Encounter.Creatures.Clear();
            foreach (var kvp in EncounterCreaturesByCount)
            {
                for (var i = 0; i < kvp.Value; i++)
                    Encounter.Encounter.Creatures.Add(kvp.Key.Creature);
            }

            await _dataService.SaveAddAsync(Encounter.Encounter);
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

    partial void OnSelectedPartyChanged(Party? value)
    {
        SetDifficulty();
    }

    private void PartyOrCreaturesChanged(object? sender, EventArgs e)
    {
        SetDifficulty();
    }

    [RelayCommand]
    private void RemoveCreature(object obj)
    {
        //if (obj != null && obj is CreatureViewModel && Encounter != null)
        if (obj != null && Encounter != null && obj is ObservableKVP<ObservableCreature, int> kvp)
        {
            ObservableCreature toRemove = kvp.Key;

            var match = EncounterCreaturesByCount.FirstOrDefault(x => x.Key.Creature.Equals(toRemove.Creature));
            if (match != null)
                EncounterCreaturesByCount.Remove(match);

            while (Encounter.Encounter.Creatures.Contains(toRemove.Creature))
                _encounterService.RemoveCreature(Encounter.Encounter, toRemove.Creature);
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
            _encounterService.CalculateEncounterXP(Encounter?.Encounter);
            ExpectedDifficulty = _encounterService.DetermineDifficultyForParty(Encounter?.Encounter, SelectedParty); //todo: possible issue here where the number of creatures doesn't reflect the count. TBD.
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
                var creature = new ObservableCreature(copied);
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
        if (parameter is ObservableCreature)
        {
            //todo: pass a copy of the creature rather than the original, so changes are discarded if user hits back button rather than committing changes.
            //_navigationService.NavigateTo(typeof(CreatureEditViewModel).FullName!, ((CreatureViewModel)parameter).Creature);

            //experiemental:
            _navigationService.NavigateTo(typeof(CreatureEditNavigationPageViewModel).FullName!, ((ObservableCreature)parameter).Creature);
        }
    }

    private void RecountEncounterCreatures()
    {
        EncounterCreatureCount = 0;
        Encounter?.Encounter.Creatures.Clear();

        foreach (var kvp in EncounterCreaturesByCount)
        {
            EncounterCreatureCount += kvp.Value;

            for (var i = 0; i < kvp.Value; i++)
                Encounter?.Encounter.Creatures.Add(kvp.Key.Creature);
        }
        SetDifficulty();
    }
}
