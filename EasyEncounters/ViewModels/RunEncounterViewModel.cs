using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Messages;
using Microsoft.UI.Xaml.Controls;

namespace EasyEncounters.ViewModels;

public partial class RunEncounterViewModel : ObservableRecipient, INavigationAware
{
    private readonly IActiveEncounterService _activeEncounterService;
    private readonly IDataService _dataService;
    private readonly INavigationService _navigationService;
    private readonly List<ActiveEncounterCreatureViewModel> _targeted;
    private ActiveEncounter? _activeEncounter;

    [ObservableProperty]
    private bool _initNotRolled;

    [ObservableProperty]
    private bool _initRolled;

    public RunEncounterViewModel(INavigationService navigationService, IDataService dataService, IActiveEncounterService activeEncounterService)
    {
        _dataService = dataService;
        _navigationService = navigationService;
        _activeEncounterService = activeEncounterService;
        InitNotRolled = true;
        _targeted = new();

        WeakReferenceMessenger.Default.Register<DamageSourceSelectedMessage>(this, (r, m) =>
        {
            DealDamage(m.Value);
        });
    }

    public ObservableCollection<ActiveEncounterCreatureViewModel> Creatures { get; private set; } = new();

    public ObservableCollection<string> Log
    {
        get; private set;
    } = new();

    public void OnNavigatedFrom()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is ActiveEncounter && parameter != null)
        {
            _activeEncounter = (ActiveEncounter)parameter;
        }
        else
        {
            _activeEncounter = await _dataService.GetActiveEncounterAsync();
        }

        Creatures.Clear();

        foreach (var p in _activeEncounter.CreatureTurns)
        {
            Creatures.Add(new ActiveEncounterCreatureViewModel(p));
        }

        foreach (var s in _activeEncounter.Log.Reverse<string>())
            Log.Add(s);

        if (_activeEncounter.ActiveTurn != null)
        {
            InitNotRolled = false;
            InitRolled = true;

            var correctTurn = _activeEncounter.ActiveTurn;
            while (_activeEncounter.CreatureTurns.Peek() != correctTurn)
                await NextTurn();

            //encounter in progress
            while (_activeEncounter.ActiveTurn.Dead)
                await NextTurn();
        }
    }

    [RelayCommand]
    private void DealDamage(ActiveEncounterCreatureViewModel sourceCreature)
    {
        if (_activeEncounter == null)
            return;

        _navigationService.NavigateTo(typeof(TargetedDamageViewModel).FullName!, new DealDamageTargetting(_activeEncounter, sourceCreature, Creatures), clearNavigation: true);
    }

    [RelayCommand]
    private async Task EndEncounter()
    {
        await _activeEncounterService.EndEncounterAsync(_activeEncounter);
        if (_navigationService.CanGoBack)
            _navigationService.GoBack();
        else //if we didn't come from encounter selection, then go all the way back to campaign select (default main page)
            _navigationService.NavigateTo(typeof(CampaignSplashViewModel).FullName!, clearNavigation: true);
    }

    [RelayCommand]
    private async Task NextTurn()
    {
        if (_activeEncounter == null)
            return;

        await _activeEncounterService.EndCurrentTurnAsync(_activeEncounter);

        //show current turn order and remove inactive creatures:
        var tmp = new List<ActiveEncounterCreatureViewModel>(Creatures);
        Creatures.Clear();
        foreach (var creature in _activeEncounter.CreatureTurns)
        {
            Creatures.Add(tmp.First(x => x.Creature.EncounterID == creature.EncounterID));
        }
    }

    [RelayCommand]
    private async Task RollInitiative()
    {
        if (_activeEncounter == null)
            return;

        if (InitNotRolled)
        {
            await _activeEncounterService.UpdateInitiativeOrder(_activeEncounter);
        }
        InitNotRolled = false;
        InitRolled = true;

        var tmp = new List<ActiveEncounterCreatureViewModel>(Creatures);
        Creatures.Clear();
        foreach (var creature in _activeEncounter.CreatureTurns)
        {
            Creatures.Add(tmp.First(x => x.Creature.EncounterID == creature.EncounterID));
        }

        await _dataService.SaveAddAsync(_activeEncounter);
    }

    [RelayCommand]
    private void SelectionChanged(SelectionChangedEventArgs e)
    {
        foreach (var added in e.AddedItems)
        {
            if (added is ActiveEncounterCreatureViewModel)
                _targeted.Add((ActiveEncounterCreatureViewModel)added);
        }
        foreach (var removed in e.RemovedItems)
        {
            if (removed is ActiveEncounterCreatureViewModel)
                _targeted.Remove((ActiveEncounterCreatureViewModel)removed);
        }
    }
}