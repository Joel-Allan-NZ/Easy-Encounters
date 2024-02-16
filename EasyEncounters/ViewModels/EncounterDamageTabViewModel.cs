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
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Messages;
using EasyEncounters.Models;
using Microsoft.AppCenter.Channel;
using Newtonsoft.Json.Linq;
using Windows.ApplicationModel.Contacts;
using Windows.UI.Notifications;

namespace EasyEncounters.ViewModels;
public partial class EncounterDamageTabViewModel : ObservableRecipientTab //ObservableRecipient, INavigationAware
{
    private readonly IList<DamageType> _damageTypes = Enum.GetValues(typeof(DamageType)).Cast<DamageType>().ToList();
    private readonly IActiveEncounterService _activeEncounterService;

    [ObservableProperty]
    private ObservableActiveAbility? _selectedAbility;

    [ObservableProperty]
    private int _damage;

    [ObservableProperty]
    private bool _hasSelectedAbility;

    public ObservableCollection<ActiveEncounterCreatureViewModel> SelectableCreatures
    {
        get;
        private set;
    } = new();

    public List<ActiveEncounterCreatureViewModel> SelectedCreatures
    {
        get; private set;
    } = new();

    public IList<DamageType> DamageTypes => _damageTypes;

    [ObservableProperty]
    private DamageType _selectedDamageType;

    partial void OnSelectedDamageTypeChanged(DamageType value)
    {
        foreach(var target in Targets)
        {
            target.SelectedDamageVolume = _activeEncounterService.GetDamageVolumeSuggestion(target.ActiveEncounterCreatureViewModel!.Creature, value);
        }
    }

    [ObservableProperty]
    private ActiveEncounter? _encounter;

    [ObservableProperty]
    private ActiveEncounterCreatureViewModel? _sourceCreature;

    public ObservableCollection<DamageCreatureViewModel> Targets
    {
        get;
        set;
    } = new();

    public EncounterDamageTabViewModel(IActiveEncounterService activeEncounterService)
    {
        _activeEncounterService = activeEncounterService;
        WeakReferenceMessenger.Default.Register<EncounterCreatureChangedMessage>(this, (r, m) =>
        {
            UpdateCreatures(m.Creatures);
        });

        WeakReferenceMessenger.Default.Register<AddTargetCreatureRequestMessage>(this, (r, m) =>
        {
            AddTargets(SelectedCreatures);
        });
        //test to see if we need to raise events for creature changes
        //WeakReferenceMessenger.Default.Register<>
    }
    public override void OnTabClosed()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public override void OnTabOpened(object? parameter)
    {
        if (parameter != null && parameter is EncounterDamageTabData data)
        {
            SelectableCreatures.Clear();
            foreach (var creature in data.Targets)
                SelectableCreatures.Add(creature);
            Encounter = data.ActiveEncounter;
            SourceCreature = data.Source;

            if(data.Ability != null)
            {
                SelectedAbility = data.Ability;
                HasSelectedAbility = true;
                SelectedDamageType = SelectedAbility.DamageType;
            }
            
        }
    }

    private void RemoveTarget(DamageCreatureViewModel target)
    {
        if (Targets.Contains(target))
            Targets.Remove(target);
    }

    [RelayCommand]
    private void AddTargets(List<ActiveEncounterCreatureViewModel> targets)
    {
        var existingTargets = new HashSet<ActiveEncounterCreature>();
        foreach (var existingTarget in Targets)
            existingTargets.Add(existingTarget.ActiveEncounterCreatureViewModel.Creature);

        foreach(var target in targets)
        {
            if (!existingTargets.Contains(target.Creature))
            {
                Targets.Add(new DamageCreatureViewModel(target));
            }
        }
    }

    [RelayCommand]
    private void SelectionChanged(object s)
    {
        //IList<object>? items = s as IList<object>;
        //if (s == null)
        //    return;

        if (s is not IList<object> items)
            return;


        SelectedCreatures.Clear();
        var existingTargets = Targets.Select(x => x.ActiveEncounterCreatureViewModel);
        var castSelection = new List<ActiveEncounterCreatureViewModel>();
        //Targets.Clear();
        foreach (var item in items)
        {
            var selection = (ActiveEncounterCreatureViewModel)item;
            castSelection.Add(selection);
            
            //foreach (var item in selection)
            SelectedCreatures.Add(selection);
            if(!existingTargets.Contains(selection))  
                Targets.Add(new DamageCreatureViewModel(selection)); //todo: cache this ahead of time in a dictionary, and just add/remove as necessary.

        }
        var toRemove = new List<DamageCreatureViewModel>();
        foreach(var existingTarget in Targets)
        {
            if (!castSelection.Contains(existingTarget.ActiveEncounterCreatureViewModel))
                toRemove.Add(existingTarget);
                
        }
        foreach(var existingTarget in toRemove)
        {
            Targets.Remove(existingTarget);
        }
        foreach (var target in Targets)
        {
            target.SelectedDamageVolume = _activeEncounterService.GetDamageVolumeSuggestion(target.ActiveEncounterCreatureViewModel!.Creature, SelectedDamageType);
        }
    }

    [RelayCommand]
    private void DealDamage()
    {
        if(SourceCreature != null)
            WeakReferenceMessenger.Default.Send(new DealDamageRequestMessage(Targets.ToList(), SourceCreature.Creature, Damage, SelectedDamageType));
    }

    [RelayCommand]
    private void RequestInspect(ActiveEncounterCreatureViewModel creatureVM)
    {
        WeakReferenceMessenger.Default.Send(new InspectRequestMessage(creatureVM));
    }

    private void UpdateCreatures(IList<ActiveEncounterCreatureViewModel> creatures)
    {
        SelectableCreatures.Clear();
        foreach (var creature in creatures)
            SelectableCreatures.Add(creature);

        var targets = new List<DamageCreatureViewModel>(Targets);

        Targets.Clear();
        foreach(var target in targets)
        {
            if (SelectableCreatures.Contains(target.ActiveEncounterCreatureViewModel))
            {
                Targets.Add(target);
            }
        }
    }


}
