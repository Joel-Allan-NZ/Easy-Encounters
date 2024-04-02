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
using Microsoft.EntityFrameworkCore;
using Windows.Media.Audio;

namespace EasyEncounters.ViewModels;

public partial class EncounterEditNavigationViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService _dataService;

    private readonly IEncounterService _encounterService;

    private readonly IFilteringService _filteringService;

    private readonly INavigationService _navigationService;


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
        _creatureFilterValues = (CreatureFilter)_filteringService.GetFilterValues<Creature>();
    }

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
        if (parameter != null && parameter is ObservableEncounter observable)
        {
            var enc = await _dataService.Encounters().Where(x => x.Id == observable.Encounter.Id).Include(x => x.Creatures).Include(x => x.CreaturesByCount).FirstAsync();
            Encounter = new ObservableEncounter(enc);

            EncounterCreaturesByCount.Clear();          
            foreach(var encounterCreature in Encounter.Encounter.CreaturesByCount)
            {
                EncounterCreaturesByCount.Add(new(new(encounterCreature.Creature), encounterCreature.Count));
            }
            //foreach (var creature in Encounter.Encounter.Creatures)
            //{
            //    var match = EncounterCreaturesByCount.FirstOrDefault(x => x.Key.Creature.Equals(creature));

            //    if (match != null)
            //    {

            //        match.Value++;
            //    }
            //    else
            //    {
                    
            //        EncounterCreaturesByCount.Add(new(new ObservableCreature(creature), 1));
            //    }
            //}
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

        foreach (var party in await _dataService.GetAllPartiesAsync()) //todo: move this to filter too. Currently okay, but problematic with large numbers of parties
        {
            Parties.Add(party);
        }

        ExpectedDifficulty = EncounterDifficulty.None;
        CreatureFilterValues = (CreatureFilter)_filteringService.GetFilterValues<Creature>();
        await CreatureFilterValues.ResetAsync();

        var campaigns = await _dataService.GetAllCampaignsAsync(); //todo: move this to filter too. Currently okay, but problematic with large numbers of campaigns

        foreach (var campaign in campaigns)
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

            //_encounterService.AddCreature(Encounter.Encounter, creature.Creature); //todo: switch this to dictionary?
        }
    }

    [RelayCommand]
    private async Task CommitChanges(object obj)
    {
        if (Encounter != null)
        //update encounter creature counts to match the kvp version
        {
            //Encounter.Encounter.CreaturesByCount.Clear();
            foreach(var kvp in EncounterCreaturesByCount)
            {
                var match = Encounter.Encounter.CreaturesByCount.FirstOrDefault(x => x.Creature.Id == kvp.Key.Creature.Id);
                if(match == null)
                {
                    var creatureMatch = await _dataService.Creatures().FirstOrDefaultAsync(x => x.Id == kvp.Key.Creature.Id);
                    var encounterPair = new EncounterCreatures(creatureMatch, kvp.Value);
                    await _dataService.SaveAddAsync(encounterPair);
                    Encounter.Encounter.CreaturesByCount.Add(encounterPair);
                }
                else if (match.Count != kvp.Value)
                {
                    match.Count = kvp.Value;
                }
            }
            var toRemove = new List<EncounterCreatures>();
            foreach(var kvp in Encounter.Encounter.CreaturesByCount)
            {
                var match = EncounterCreaturesByCount.FirstOrDefault(x => x.Key.Creature.Id == kvp.Creature.Id);
                if(match == null)
                {
                    toRemove.Add(kvp);
                }
            }
            foreach(var remove in toRemove)
            {
                Encounter.Encounter.CreaturesByCount.Remove(remove);
            }
            //Encounter.Encounter.Creatures.Clear();
            //foreach (var kvp in EncounterCreaturesByCount)
            //{
            //    for (var i = 0; i < kvp.Value; i++)
            //        Encounter.Encounter.Creatures.Add(kvp.Key.Creature);
            //}
            Encounter.Encounter.CreatureCount = Encounter.Encounter.CreaturesByCount.Sum(x => x.Count);

            await _dataService.SaveAddAsync(Encounter.Encounter);
            if (_navigationService.CanGoBack)
                _navigationService.GoBack();
        }
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

    private void SetDifficulty()
    {
        if (SelectedParty == null || SelectedParty.Members.Count == 0 || EncounterCreaturesByCount.Count == 0)
            ExpectedDifficulty = EncounterDifficulty.None;
        else
        {
            _encounterService.CalculateEncounterXP(Encounter?.Encounter);
            ExpectedDifficulty = _encounterService.DetermineDifficultyForParty(Encounter?.Encounter, SelectedParty); //todo: possible issue here where the number of creatures doesn't reflect the count. TBD.
        }
    }

    [RelayCommand]
    private async Task CopyCreature(object parameter)
    {
        if (parameter != null && parameter is Creature)
        {
            var copied = await _dataService.CopyAsync(parameter as Creature);
        }
    }


    [RelayCommand]
    private async Task DeleteCreature(object parameter)
    {
        if (parameter != null && parameter is Creature creature)
        {
            var match = await _dataService.Encounters().FirstOrDefaultAsync(x => x.Id == creature.Id);
            if (match != null)
            {
                await _dataService.DeleteAsync(match);
            }
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
