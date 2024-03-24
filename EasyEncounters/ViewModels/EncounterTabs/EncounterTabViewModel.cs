using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Messages;
using EasyEncounters.Models;
using EasyEncounters.ViewModels.EncounterTabs;

namespace EasyEncounters.ViewModels;

public partial class EncounterTabViewModel : ObservableRecipient, INavigationAware
{
    private readonly IActiveEncounterService _activeEncounterService;
    private readonly IDataService _dataService;
    private readonly INavigationService _navigationService;
    private readonly ITabService _tabService;
    private ActiveEncounter? _activeEncounter;

    [ObservableProperty]
    private bool _initiativeRolled;

    [ObservableProperty]
    private ObservableRecipientTab? _selectedTab;

    public EncounterTabViewModel(ITabService tabService, IActiveEncounterService activeEncounterService,
        INavigationService navigationService, IDataService dataService)
    {
        _tabService = tabService;
        _activeEncounterService = activeEncounterService;
        _navigationService = navigationService;
        _activeEncounterService = activeEncounterService;
        _dataService = dataService;

        WeakReferenceMessenger.Default.Register<AbilityDamageRequestMessage>(this, (r, m) =>
        {
            ShowDamageTab(m.Ability, m.SourceCreature);
        });

        WeakReferenceMessenger.Default.Register<DamageSourceSelectedMessage>(this, (r, m) =>
        {
            ShowDamageTab(null, m.Value);
        });

        WeakReferenceMessenger.Default.Register<InspectRequestMessage>(this, (r, m) =>
        {
            ShowCreatureDisplayTab(m.Parameter);
        });

        WeakReferenceMessenger.Default.Register<DealDamageRequestMessage>(this, (r, m) =>
        {
            DealDamage(m);
        });

        WeakReferenceMessenger.Default.Register<AddCreaturesRequestMessage>(this, (r, m) =>
        {
            AddCreatures(m.CreaturesToAdd);
        });
        Tabs.Add(_tabService.OpenTab(typeof(LogTabViewModel).FullName!, null, "Log", false));
    }

    public ObservableCollection<ObservableActiveEncounterCreature> Creatures
    {
        get; private set;
    } = new();

    public ObservableCollection<ObservableRecipientTab> Tabs
    {
        get; private set;
    } = new();

    public void OnNavigatedFrom()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public async void OnNavigatedTo(object activeEncounter)
    {
        if (activeEncounter != null && activeEncounter is ActiveEncounter)
        {
            _activeEncounter = (ActiveEncounter)activeEncounter;
            WeakReferenceMessenger.Default.Send(new LogMessageLogged(new List<string>() { "Encounter Begins!" }));
        }
        else
        {
            //_activeEncounter = await _dataService.GetActiveEncounterAsync();
        }
        Creatures.Clear();

        foreach (var creature in _activeEncounter.ActiveCreatures)
            Creatures.Add(new ObservableActiveEncounterCreature(creature));

        //foreach (var combatLogString in _activeEncounter.Log.Reverse<string>())
        //    CombatLog.Add(combatLogString);
    }

    [RelayCommand]
    private void AddCreatures(ICollection<ObservableKVP<ObservableCreature, int>> creaturesToAdd)
    {
        if (_activeEncounter == null)
            return;

        foreach (var kvp in creaturesToAdd)
        {
            for (var i = 0; i < kvp.Value; i++)
            {
                _activeEncounterService.AddCreatureToInProgressEncounter(_activeEncounter, kvp.Key.Creature);
                Creatures.Add(new ObservableActiveEncounterCreature(_activeEncounter.ActiveCreatures.Last()));
            }
        }
    }

    [RelayCommand]
    private void CloseTab(object obj)
    {
        if (obj != null && obj is ObservableRecipientTab)
        {
            var tab = (ObservableRecipientTab)obj;
            if (tab.IsClosable)
            {
                Tabs.Remove(tab);
                _tabService.CloseTab(tab);
            }
        }
    }

    private async void DealDamage(DealDamageRequestMessage request)
    {
        var msg = new List<string>();
        foreach (var target in request.Targets)
        {
            msg.Add(await _activeEncounterService.DealDamageAsync(_activeEncounter, new DamageInstance(target.ActiveEncounterCreatureViewModel.Creature, request.SourceCreature, request.DamageType,
                target.SelectedDamageVolume, request.BaseDamage)));

            target.ActiveEncounterCreatureViewModel.CurrentHP = target.ActiveEncounterCreatureViewModel.Creature.CurrentHP; //hax. TODO: better.
        }

        WeakReferenceMessenger.Default.Send(new LogMessageLogged(msg));
    }

    [RelayCommand]
    private async Task EndEncounter()
    {
        await _activeEncounterService.EndEncounterAsync(_activeEncounter);
        foreach (var tab in Tabs)
        {
            tab.OnTabClosed();
        }
        if (_navigationService.CanGoBack)
            _navigationService.GoBack();
        else
            _navigationService.NavigateTo(typeof(CampaignSplashViewModel).FullName!, clearNavigation: true);
    }

    [RelayCommand]
    private async Task NextTurn()
    {
        if (_activeEncounter == null)
            return;

        var current = await _activeEncounterService.EndCurrentTurnAsync(_activeEncounter); 

        if (_activeEncounter.CreatureTurns.Count != Creatures.Count)
        {
            var temp = new List<ObservableActiveEncounterCreature>(Creatures);
            Creatures.Clear();
            foreach (var creature in temp)
            {
                foreach (var creatureTurn in _activeEncounter.CreatureTurns)
                {
                    if (creature.IsWrapperFor(creatureTurn))
                    {
                        if (creatureTurn == _activeEncounter.ActiveTurn)
                        {
                            creature.Targeted = true;
                            ShowCreatureDisplayTab(creature);
                        }
                        else
                            creature.Targeted = false;

                        Creatures.Add(creature);
                        break;
                    }
                }
            }
        }
        else
        {
            foreach (var creature in Creatures)
            {
                if (_activeEncounter.ActiveTurn?.Equals(creature.Creature) ?? false)
                {
                    creature.Targeted = true;
                    ShowCreatureDisplayTab(creature);
                }
                else
                    creature.Targeted = false;
            }
        }

        WeakReferenceMessenger.Default.Send(new LogMessageLogged(new List<string>() { current }));
        WeakReferenceMessenger.Default.Send(new EncounterCreatureChangedMessage(Creatures));
    }

    [RelayCommand]
    private void ReportReorder()
    {
        _activeEncounterService.OrderCreatureTurns(_activeEncounter, Creatures.Select(x => x.Creature));
    }

    [RelayCommand]
    private async Task RollInitiative()
    {
        var orderedInitiative = await _activeEncounterService.RollInitiative(_activeEncounter);

        var tempAECreatureList = new List<ObservableActiveEncounterCreature>(Creatures);
        Creatures.Clear();

        foreach (var orderedCreature in orderedInitiative)
        {
            foreach (var creature in tempAECreatureList)
            {
                if (creature.IsWrapperFor(orderedCreature))
                {
                    //todo: improve this hacky fix
                    Creatures.Add(creature);
                    var ph = creature.Initiative;
                    creature.Initiative = 1;
                    creature.Initiative = ph;

                    break;
                }
            }
        }
        Creatures.First().Targeted = true;
        ShowCreatureDisplayTab(Creatures.First());

        if (_activeEncounter != null)
            await _dataService.SaveAddAsync(_activeEncounter);

        InitiativeRolled = true;

        WeakReferenceMessenger.Default.Send(new LogMessageLogged(new List<string>() { "Initiative Rolled!", $"{_activeEncounter?.ActiveTurn?.EncounterName ?? "A creature"}'s turn" }));
    }

    [RelayCommand]
    private void ShowAddCreatureTab()
    {
        var openTab = Tabs.FirstOrDefault(x => x.TabName == "Add Creatures");
        if (openTab == null)
        {
            openTab = _tabService.OpenTab(typeof(EncounterAddCreaturesTabViewModel).FullName!,
                _activeEncounter, "Add Creatures");

            Tabs.Add(openTab);
        }

        SelectedTab = openTab;
    }

    [RelayCommand]
    private void ShowCreatureDisplayTab(ObservableActiveEncounterCreature obj)
    {
        if (obj != null && obj is ObservableActiveEncounterCreature)
        {
        
            var openTab = Tabs.FirstOrDefault(x => x.TabName == obj.Creature.EncounterName);

            if (openTab == null)
            {
                openTab = _tabService.OpenTab(typeof(CreatureDisplayTabViewModel).FullName!, obj, obj.Creature.EncounterName);
                Tabs.Add(openTab);
            }
            SelectedTab = openTab;
        }
    }

    private void ShowDamageTab(ObservableActiveAbility? abilityVM, ObservableActiveEncounterCreature source)
    {
        if (_activeEncounter == null)
            return;

        var openTab = Tabs.FirstOrDefault(x => x.TabName == $"Damage from {source.Creature.EncounterName}");
        if (openTab == null)
        {
            if (abilityVM != null)
                openTab = _tabService.OpenTab(typeof(EncounterDamageTabViewModel).FullName!,
                    new EncounterDamageTabData(_activeEncounter, source, Creatures, abilityVM), $"Damage from {source.Creature.EncounterName}");
            else
                openTab = _tabService.OpenTab(typeof(EncounterDamageTabViewModel).FullName!,
                    new EncounterDamageTabData(_activeEncounter, source, Creatures, null), $"Damage from {source.Creature.EncounterName}");

            Tabs.Add(openTab);
        }
        else
        {
            ((EncounterDamageTabViewModel)openTab).SelectedAbility = abilityVM;
            ((EncounterDamageTabViewModel)openTab).HasSelectedAbility = abilityVM != null;
            ((EncounterDamageTabViewModel)openTab).SelectedDamageType = abilityVM?.DamageType ?? Core.Models.Enums.DamageType.None;
        }
        SelectedTab = openTab;
    }
}