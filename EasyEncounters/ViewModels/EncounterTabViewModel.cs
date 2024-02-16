using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Messages;
using EasyEncounters.Models;
using EasyEncounters.Views;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.Web.UI;

namespace EasyEncounters.ViewModels;

public partial class EncounterTabViewModel : ObservableRecipient, INavigationAware
{
    private readonly ITabService _tabService;
    private readonly INavigationService _navigationService;
    private readonly IDataService _dataService;
    private readonly IActiveEncounterService _activeEncounterService;

    private ActiveEncounter? _activeEncounter;

    [ObservableProperty]
    private ObservableRecipientTab? _selectedTab;

    [ObservableProperty]
    private bool _initiativeRolled;

    public ObservableCollection<ActiveEncounterCreatureViewModel> Creatures
    {
        get; private set;
    } = new();

    public ObservableCollection<ObservableRecipientTab> Tabs
    {
        get; private set;
    } = new();

    [RelayCommand]
    private async Task RollInitiative()
    {
        var orderedInitiative = await _activeEncounterService.UpdateInitiativeOrder(_activeEncounter);

        var tempAECreatureList = new List<ActiveEncounterCreatureViewModel>(Creatures);
        Creatures.Clear();

        foreach (var orderedCreature in orderedInitiative)
        {
            foreach (var creature in tempAECreatureList)
            {
                if (creature.IsWrapperFor(orderedCreature))
                {
                    //todo: improve this hacky fix
                    //creature.Initiative = orderedCreature.Initiative;
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
       
        if(_activeEncounter != null)
            await _dataService.SaveAddAsync(_activeEncounter);

        InitiativeRolled = true;

        WeakReferenceMessenger.Default.Send(new LogMessageLogged(new List<string>() { "Initiative Rolled!", $"{_activeEncounter?.ActiveTurn?.EncounterName ?? "A creature"}'s turn" }));
    }

    [RelayCommand]
    private void AddCreatures(ICollection<ObservableKVP<CreatureViewModel, int>> creaturesToAdd)
    {
        if (_activeEncounter == null)
            return;

        foreach(var kvp in creaturesToAdd)
        {
            for(var i =0;i<kvp.Value; i++)
            {
                _activeEncounterService.AddCreatureInProgress(_activeEncounter, kvp.Key.Creature);
                Creatures.Add(new ActiveEncounterCreatureViewModel(_activeEncounter.ActiveCreatures.Last()));
            }
        }
    }

    [RelayCommand]
    private void ReportReorder()
    {
        _activeEncounterService.ReorderInitiative(_activeEncounter, Creatures.Select(x => x.Creature));
    }

    [RelayCommand]
    private async Task NextTurn()
    {
        if (_activeEncounter == null)
            return;

        var current = await _activeEncounterService.EndCurrentTurnAsync(_activeEncounter); /*$"{_activeEncounter.ActiveTurn?.EncounterName ?? "A creature"} ends their turn.";*/


        if (_activeEncounter.CreatureTurns.Count != Creatures.Count)
        {
            var temp = new List<ActiveEncounterCreatureViewModel>(Creatures);
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
        //string next = $"{_activeEncounter.ActiveTurn?.EncounterName ?? "Another creature"}'s turn";
        WeakReferenceMessenger.Default.Send(new LogMessageLogged(new List<string>() { current }));
        WeakReferenceMessenger.Default.Send(new EncounterCreatureChangedMessage(Creatures));
    }

    [RelayCommand]
    private async Task EndEncounter()
    {
        await _activeEncounterService.EndEncounterAsync(_activeEncounter);
        foreach(var tab in Tabs)
        {
            tab.OnTabClosed();
        }
        if (_navigationService.CanGoBack)
            _navigationService.GoBack();
        else
            _navigationService.NavigateTo(typeof(CampaignSplashViewModel).FullName!, clearNavigation: true);
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

    [RelayCommand]
    private void ShowCreatureDisplayTab(ActiveEncounterCreatureViewModel obj)
    {
        if (obj != null && obj is ActiveEncounterCreatureViewModel)
        {
            //var aecVM = obj as ActiveEncounterCreatureViewModel;

            var openTab = Tabs.FirstOrDefault(x => x.TabName == obj.Creature.EncounterName);

            if (openTab == null)
            {
                openTab = _tabService.OpenTab(typeof(CreatureDisplayTabViewModel).FullName!, obj, obj.Creature.EncounterName);
                Tabs.Add(openTab);
            }
            SelectedTab = openTab;
        }
    }

    private async void DealDamage(DealDamageRequestMessage request)
    {
        var msg = new List<string>();
        foreach (var target in request.Targets)
        {
            msg.Add( await _activeEncounterService.DealDamageAsync(_activeEncounter, new DamageInstance(target.ActiveEncounterCreatureViewModel.Creature, request.SourceCreature, request.DamageType,
                target.SelectedDamageVolume, request.BaseDamage)));
            //msg.Add(_activeEncounterService.DealDamage(Encounter, SourceCreature.Creature, target.ActiveEncounterCreatureViewModel.Creature, SelectedDamageType, Damage));
            target.ActiveEncounterCreatureViewModel.CurrentHP = target.ActiveEncounterCreatureViewModel.Creature.CurrentHP; //hax. TODO: better.

        }

        WeakReferenceMessenger.Default.Send(new LogMessageLogged(msg));
    }

    [RelayCommand]
    private void ShowAddCreatureTab()
    {
        var openTab = Tabs.FirstOrDefault(x => x.TabName == "Add Creatures");
        if(openTab == null)
        {
            openTab = _tabService.OpenTab(typeof(EncounterAddCreaturesTabViewModel).FullName!,
                _activeEncounter, "Add Creatures");

            Tabs.Add(openTab);
        }

        SelectedTab = openTab;
    }

    private void ShowDamageTab(ObservableActiveAbility? abilityVM, ActiveEncounterCreatureViewModel source)
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
            //Tuple.Create(_activeEncounter, source), $"Damage from {source.Creature.EncounterName}");

            Tabs.Add(openTab);
            //Tabs.Insert(0, openTab);
            //Tabs.Insert(1, openTab);
        }
        else
        {
            if (abilityVM != null)
            {
                ((EncounterDamageTabViewModel)openTab).SelectedAbility = abilityVM;
                ((EncounterDamageTabViewModel)openTab).HasSelectedAbility = true;
                ((EncounterDamageTabViewModel)openTab).SelectedDamageType = abilityVM.DamageType;
            }
            else
            ((EncounterDamageTabViewModel)openTab).HasSelectedAbility = false;

        }
        SelectedTab = openTab;

    }


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
        //WeakReferenceMessenger.Default.Register<LogMessageLogged>(this, (r, m) =>
        //{
        //    DamageLogged(m.LogMessages);
        //});
        Tabs.Add(_tabService.OpenTab(typeof(LogTabViewModel).FullName!, null, "Log", false));
    }

    public void OnNavigatedFrom()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public async void OnNavigatedTo(object parameter)
    {
        if(parameter != null && parameter is ActiveEncounter)
        {
            _activeEncounter = (ActiveEncounter)parameter;
        }
        else
        {
            _activeEncounter = await _dataService.GetActiveEncounterAsync();
        }
        Creatures.Clear();

        foreach (var creature in _activeEncounter.ActiveCreatures)
            Creatures.Add(new ActiveEncounterCreatureViewModel(creature));

        //foreach (var combatLogString in _activeEncounter.Log.Reverse<string>())
        //    CombatLog.Add(combatLogString);
    }
}
