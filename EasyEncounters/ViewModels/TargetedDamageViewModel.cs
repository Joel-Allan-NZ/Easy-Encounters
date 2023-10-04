using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
public partial class TargetedDamageViewModel : ObservableRecipient, INavigationAware
{

    private readonly IList<DamageType> _damageTypes = Enum.GetValues(typeof(DamageType)).Cast<DamageType>().ToList();
    public IList<DamageType> DamageTypes => _damageTypes;

    /// <summary>
    /// The list of possible target creatures
    /// </summary>
    public ObservableCollection<ActiveEncounterCreatureViewModel> TargetableCreatures
    {
        get; private set;
    } = new();

    public ObservableCollection<TargetDamageInstanceViewModel> DamageInstances
    {
        get; private set;
    } = new();

    /// <summary>
    /// workaround attempt for nested vm not binding. todo: fix properly.
    /// </summary>
    //public ObservableCollection<DamageCreatureViewModel> Targets
    //{
    //    get; private set;
    //} = new();

    /// <summary>
    /// The source of damage.
    /// </summary>
    [ObservableProperty]
    private ActiveEncounterCreatureViewModel? _sourceCreature;

    [ObservableProperty]
    private TargetDamageInstanceViewModel? _selectedTargetedDamageInstance;

    //partial void OnSelectedTargetedDamageInstanceChanged(TargetDamageInstanceViewModel? value)
    //{
    //    if(value != null)
    //    {
    //        Targets.Clear();
    //        foreach (var target in value.Targets)
    //            Targets.Add(target);
    //    }
    //}




    private readonly INavigationService _navigationService;
    private readonly IActiveEncounterService _activeEncounterService;
    private ActiveEncounter? _activeEncounter;

    public TargetedDamageViewModel(IActiveEncounterService activeEncounterService, INavigationService navigationService)
    {
        _navigationService = navigationService;
        _activeEncounterService = activeEncounterService;

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

        AddDamage();
        SelectedTargetedDamageInstance = DamageInstances.First();


    }

    public void OnNavigatedFrom()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public void OnNavigatedTo(object parameter)
    {
        if (parameter is not null && parameter is DealDamageTargetting)//source creature && list of valid targets. )
        {
            var targetting = parameter as DealDamageTargetting;
            SourceCreature = targetting!.Source;

            foreach(var target in targetting.Targets)
            {
                TargetableCreatures.Add(target);
            }

            _activeEncounter = targetting.Encounter;
        }


    }

    [RelayCommand]
    private void AddDamage()
    {
        int count = 1;
        if (DamageInstances.Count > 0)
        {
            count = DamageInstances.Last().Count + 1;
        }
        DamageInstances.Add(new TargetDamageInstanceViewModel(count));
    }

    private void CopyDamage(TargetDamageInstanceViewModel damageInstance)
    {
        AddDamage();
        var last = DamageInstances.Last();
        foreach(var target in damageInstance.Targets)
        {
            last.Targets.Add(new DamageCreatureViewModel(target.ActiveEncounterCreatureViewModel!));
        }

    }

    private void RemoveDamage(TargetDamageInstanceViewModel damageInstance)
    {
        if (DamageInstances.Count < 1)
            return;

        var index = -1;
        for(var i = 0; i<DamageInstances.Count; i++)
        {
            if (DamageInstances[i].Name == damageInstance.Name)
            {
                index = i;
                break;
            }
        }
        if (index > -1)
        {
            if (DamageInstances[index] == SelectedTargetedDamageInstance)
            {
                DamageInstances.RemoveAt(index);
                SelectedTargetedDamageInstance = DamageInstances[0];
            }
            else
                DamageInstances.RemoveAt(index);
        }
            
    }

    private void RemoveTarget(DamageCreatureViewModel creature)
    {
        if (SelectedTargetedDamageInstance != null && creature != null)
        {
            var index = -1;
            for (var i = 0; i < SelectedTargetedDamageInstance.Targets.Count; i++)
            {
                if (SelectedTargetedDamageInstance.Targets[i].ActiveEncounterCreatureViewModel!.Creature.EncounterID
                        == creature.ActiveEncounterCreatureViewModel!.Creature.EncounterID)
                {
                    index = i;
                    break;
                }
            }
            if (index > -1)
                SelectedTargetedDamageInstance.Targets.RemoveAt(index);
        }
    }

    [RelayCommand]
    private void AddTarget(ActiveEncounterCreatureViewModel creature)
    {
        if (SelectedTargetedDamageInstance != null)
        {

            bool contains = false;
            foreach (var target in SelectedTargetedDamageInstance.Targets)
            {
                if (target.ActiveEncounterCreatureViewModel!.Creature.EncounterID == creature.Creature.EncounterID)
                    contains = true;
            }
            if (!contains)
            {
                var vm = new DamageCreatureViewModel(creature);
                SelectedTargetedDamageInstance.Targets.Add(vm);
            }
        }

    }

    private void DamageTypeChanged(DamageType damageType)
    {
        if(SelectedTargetedDamageInstance != null)
        {
            foreach (var target in SelectedTargetedDamageInstance.Targets)
            {
                target.SelectedDamageVolume = _activeEncounterService.GetDamageVolumeSuggestion(target.ActiveEncounterCreatureViewModel!.Creature, damageType);
            }
        }
    }

    

    [RelayCommand]
    private void DealDamage()
    {
        var instances = GetInstancesOfDamage();
        foreach (var instance in instances)
            _activeEncounterService.DealDamage(_activeEncounter, instance);

        _navigationService.NavigateTo(typeof(RunEncounterViewModel).FullName!, ignoreNavigation: true);
    }

    private IEnumerable<DamageInstance> GetInstancesOfDamage()
    {
        List<DamageInstance> instances = new List<DamageInstance>();

        foreach (var targetedDamage in DamageInstances)
        {
            foreach(var target in targetedDamage.Targets)
            {
                instances.Add(new DamageInstance(target.ActiveEncounterCreatureViewModel!.Creature,
                                                    SourceCreature!.Creature, 
                                                    targetedDamage.SelectedDamageType, 
                                                    target.SelectedDamageVolume, 
                                                    targetedDamage.DamageAmount));
            }
        }
        return instances;
    }
}
