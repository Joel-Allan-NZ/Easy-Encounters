using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Messages;

namespace EasyEncounters.ViewModels;

//todo:
//view.

public partial class ActiveEncounterViewModel : ObservableRecipient, INavigationAware
{
    private readonly IActiveEncounterService _activeEncounterService;
    private readonly IList<DamageType> _damageTypes = Enum.GetValues(typeof(DamageType)).Cast<DamageType>().ToList();
    private readonly IDataService _dataService;
    private readonly INavigationService _navigationService;
    private ActiveEncounter? _activeEncounter;

    [ObservableProperty]
    private ActiveEncounterCreatureViewModel? _damageSourceCreature;

    [ObservableProperty]
    private ActiveEncounterCreatureViewModel? _selectedCreature;

    [ObservableProperty]
    private TargetDamageInstanceViewModel? _selectedTargetDamageInstance;

    public ActiveEncounterViewModel(INavigationService navigationService, IDataService dataService, IActiveEncounterService activeEncounterService)
    {
        _dataService = dataService;
        _navigationService = navigationService;
        _activeEncounterService = activeEncounterService;

        //Register for Messages
        WeakReferenceMessenger.Default.Register<AddTargetCreatureRequestMessage>(this, (r, m) =>
        {
            AddTarget(m.Parameter);
        });

        WeakReferenceMessenger.Default.Register<RemoveTargetCreatureRequestMessage>(this, (r, m) =>
        {
            RemoveTarget(m.Parameter);
        });

        WeakReferenceMessenger.Default.Register<TargetedDamageCopyRequestMessage>(this, (r, m) =>
        {
            CopyDamage(m.Parameter);
        });

        WeakReferenceMessenger.Default.Register<TargetedDamageDeleteRequestMessage>(this, (r, m) =>
        {
            RemoveDamage(m.Parameter);
        });

        WeakReferenceMessenger.Default.Register<DamageTypeChangedMessage>(this, (r, m) =>
        {
            DamageTypeChanged(m.Parameter);
        });

        WeakReferenceMessenger.Default.Register<DamageSourceSelectedMessage>(this, (r, m) =>
        {
            DamageSourceChangeRequested(m.Value);
        });
    }

    public ObservableCollection<string> CombatLog
    {
        get; private set;
    } = new();

    public ObservableCollection<ActiveEncounterCreatureViewModel> Creatures
    {
        get; private set;
    } = new();

    public ObservableCollection<TargetDamageInstanceViewModel> DamageInstances
    {
        get; private set;
    } = new();

    public IList<DamageType> DamageTypes => _damageTypes;

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

        foreach (var creature in _activeEncounter.CreatureTurns)
            Creatures.Add(new ActiveEncounterCreatureViewModel(creature));

        foreach (var combatLogString in _activeEncounter.Log.Reverse<string>())
            CombatLog.Add(combatLogString);
    }

    [RelayCommand]
    private void AddDamage()
    {
        if (DamageSourceCreature == null)
            return;

        var count = 1;
        if (DamageInstances.Count > 0)
        {
            count = DamageInstances.Last().Count + 1;
        }
        DamageInstances.Add(new TargetDamageInstanceViewModel(count));
    }

    [RelayCommand]
    private void AddTarget(ActiveEncounterCreatureViewModel creature)
    {
        if (SelectedTargetDamageInstance != null)
        {
            var contains = false;
            foreach (var target in SelectedTargetDamageInstance.Targets)
            {
                if (target.ActiveEncounterCreatureViewModel!.Creature.EncounterID == creature.Creature.EncounterID)
                    contains = true;
            }
            if (!contains)
            {
                var vm = new DamageCreatureViewModel(creature);
                SelectedTargetDamageInstance.Targets.Add(vm);
            }
        }
    }

    private void CopyDamage(TargetDamageInstanceViewModel damageInstance)
    {
        AddDamage();
        var last = DamageInstances.Last();
        foreach (var target in damageInstance.Targets)
        {
            last.Targets.Add(new DamageCreatureViewModel(target.ActiveEncounterCreatureViewModel!));
        }
    }

    private void DamageSourceChangeRequested(ActiveEncounterCreatureViewModel creature)
    {
        DamageSourceCreature = creature;
        DamageInstances.Clear();
        DamageInstances.Add(new TargetDamageInstanceViewModel(1));
        SelectedTargetDamageInstance = DamageInstances.First();
    }

    private void DamageTypeChanged(DamageType damageType)
    {
        if (SelectedTargetDamageInstance != null)
        {
            foreach (var target in SelectedTargetDamageInstance.Targets)
            {
                target.SelectedDamageVolume = _activeEncounterService.GetDamageVolumeSuggestion(target.ActiveEncounterCreatureViewModel!.Creature, damageType);
            }
        }
    }

    [RelayCommand]
    private void DealDamage()
    {
        if (_activeEncounter != null)
        {
            var instances = GetInstancesOfDamage();
            foreach (var instance in instances)
            {
                _activeEncounterService.DealDamageAsync(_activeEncounter, instance);
                CombatLog.Insert(0, _activeEncounter.Log.Last());
            }

            SelectedTargetDamageInstance?.Targets.Clear();
            SelectedTargetDamageInstance = null;
            DamageInstances.Clear();
        }
    }

    [RelayCommand]
    private async Task EndEncounter()
    {
        await _activeEncounterService.EndEncounterAsync(_activeEncounter);
        if (_navigationService.CanGoBack)
            _navigationService.GoBack();
        else
            _navigationService.NavigateTo(typeof(CampaignSplashViewModel).FullName!, clearNavigation: true);
    }

    private IEnumerable<DamageInstance> GetInstancesOfDamage()
    {
        List<DamageInstance> instances = new List<DamageInstance>();

        foreach (var targetedDamage in DamageInstances)
        {
            foreach (var target in targetedDamage.Targets)
            {
                instances.Add(new DamageInstance(target.ActiveEncounterCreatureViewModel!.Creature,
                                                    DamageSourceCreature!.Creature,
                                                    targetedDamage.SelectedDamageType,
                                                    target.SelectedDamageVolume,
                                                    targetedDamage.DamageAmount));
            }
        }
        return instances;
    }

    [RelayCommand]
    private async Task NextTurn()
    {
        //await _activeEncounterService.EndCurrentTurnAsync(_activeEncounter);

        //var temp = new List<ActiveEncounterCreatureViewModel>(Creatures);
        //Creatures.Clear();
        //foreach (var creatureTurn in _activeEncounter.CreatureTurns)
        //{
        //    foreach(var creature in temp)
        //    {
        //        if (creature.IsWrapperFor(creatureTurn))
        //        {
        //            Creatures.Add(creature);
        //            break;
        //        }
        //    }
        //}
        if (_activeEncounter != null)
        {
            await _activeEncounterService.EndCurrentTurnAsync(_activeEncounter);

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
                        creature.Targeted = true;
                    else
                        creature.Targeted = false;
                }
            }
        }
    }

    private void RemoveDamage(TargetDamageInstanceViewModel damageInstance)
    {
        if (DamageInstances.Count < 1)
            return;

        var index = -1;
        for (var i = 0; i < DamageInstances.Count; i++)
        {
            if (DamageInstances[i].Name == damageInstance.Name)
            {
                index = i;
                break;
            }
        }
        if (index > -1)
        {
            if (DamageInstances[index] == SelectedTargetDamageInstance)
            {
                DamageInstances.RemoveAt(index);
                SelectedTargetDamageInstance = DamageInstances[0];
            }
            else
                DamageInstances.RemoveAt(index);
        }
    }

    private void RemoveTarget(DamageCreatureViewModel creature)
    {
        if (SelectedTargetDamageInstance != null && creature != null)
        {
            var index = -1;
            for (var i = 0; i < SelectedTargetDamageInstance.Targets.Count; i++)
            {
                if (SelectedTargetDamageInstance.Targets[i].ActiveEncounterCreatureViewModel!.Creature.EncounterID
                        == creature.ActiveEncounterCreatureViewModel!.Creature.EncounterID)
                {
                    index = i;
                    break;
                }
            }
            if (index > -1)
                SelectedTargetDamageInstance.Targets.RemoveAt(index);
        }
    }

    [RelayCommand]
    private async Task RollInitiative()
    {
        var orderedInitiative = await _activeEncounterService.RollInitiative(_activeEncounter);

        var tempAECreatureList = new List<ActiveEncounterCreatureViewModel>(Creatures);
        Creatures.Clear();

        foreach (var orderedCreature in orderedInitiative)
        {
            foreach (var creature in tempAECreatureList)
            {
                if (creature.IsWrapperFor(orderedCreature))
                {
                    Creatures.Add(creature);
                    break;
                }
            }
        }

        await _dataService.SaveAddAsync(_activeEncounter);
    }
}