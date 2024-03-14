using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Messages;
using EasyEncounters.Models;

namespace EasyEncounters.ViewModels;

public partial class EncounterDamageTabViewModel : ObservableRecipientTab //ObservableRecipient, INavigationAware
{
    private readonly IActiveEncounterService _activeEncounterService;
    private readonly IList<DamageType> _damageTypes = Enum.GetValues(typeof(DamageType)).Cast<DamageType>().ToList();

    [ObservableProperty]
    private int _damage;

    [ObservableProperty]
    private ActiveEncounter? _encounter;

    [ObservableProperty]
    private bool _hasSelectedAbility;

    [ObservableProperty]
    private ObservableActiveAbility? _selectedAbility;

    [ObservableProperty]
    private DamageType _selectedDamageType;

    [ObservableProperty]
    private ObservableActiveEncounterCreature? _sourceCreature;

    public EncounterDamageTabViewModel(IActiveEncounterService activeEncounterService)
    {
        _activeEncounterService = activeEncounterService;
        WeakReferenceMessenger.Default.Register<EncounterCreatureChangedMessage>(this, (r, m) =>
        {
            UpdateCreatures(m.Creatures);
        });

        //WeakReferenceMessenger.Default.Register<AddTargetCreatureRequestMessage>(this, (r, m) =>
        //{
        //    AddTargets(SelectedCreatures);
        //});
        //test to see if we need to raise events for creature changes
        //WeakReferenceMessenger.Default.Register<>
    }

    public IList<DamageType> DamageTypes => _damageTypes;

    public ObservableCollection<ObservableActiveEncounterCreature> SelectableCreatures
    {
        get;
        private set;
    } = new();

    public List<ObservableActiveEncounterCreature> SelectedCreatures
    {
        get; private set;
    } = new();

    public ObservableCollection<DamageCreatureViewModel> Targets
    {
        get;
        set;
    } = new();

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

            if (data.Ability != null)
            {
                SelectedAbility = data.Ability;
                HasSelectedAbility = true;
                SelectedDamageType = SelectedAbility.DamageType;
            }
        }
    }

    [RelayCommand]
    private void AddTargets(List<ObservableActiveEncounterCreature> targets)
    {
        var existingTargets = new HashSet<ActiveEncounterCreature>();
        foreach (var existingTarget in Targets)
            existingTargets.Add(existingTarget.ActiveEncounterCreatureViewModel.Creature);

        foreach (var target in targets)
        {
            if (!existingTargets.Contains(target.Creature))
            {
                Targets.Add(new DamageCreatureViewModel(target));
            }
        }
    }

    [RelayCommand]
    private void DealDamage()
    {
        if (SourceCreature != null)
            WeakReferenceMessenger.Default.Send(new DealDamageRequestMessage(Targets.ToList(), SourceCreature.Creature, Damage, SelectedDamageType));
    }

    partial void OnSelectedDamageTypeChanged(DamageType value)
    {
        foreach (var target in Targets)
        {
            target.SelectedDamageVolume = _activeEncounterService.GetDamageVolumeSuggestion(target.ActiveEncounterCreatureViewModel!.Creature, value);
        }
    }

    private void RemoveTarget(DamageCreatureViewModel target)
    {
        if (Targets.Contains(target))
            Targets.Remove(target);
    }

    [RelayCommand]
    private void RequestInspect(ObservableActiveEncounterCreature creatureVM)
    {
        WeakReferenceMessenger.Default.Send(new InspectRequestMessage(creatureVM));
    }

    [RelayCommand]
    private void SelectionChanged(object s)
    {

        if (s is not IList<object> items)
            return;

        SelectedCreatures.Clear();
        var existingTargets = Targets.Select(x => x.ActiveEncounterCreatureViewModel);
        var castSelection = new List<ObservableActiveEncounterCreature>();
        //Targets.Clear();
        foreach (var item in items)
        {
            var selection = (ObservableActiveEncounterCreature)item;
            castSelection.Add(selection);

            //foreach (var item in selection)
            SelectedCreatures.Add(selection);
            if (!existingTargets.Contains(selection))
                Targets.Add(new DamageCreatureViewModel(selection)); //todo: cache this ahead of time in a dictionary, and just add/remove as necessary.
        }
        var toRemove = new List<DamageCreatureViewModel>();
        foreach (var existingTarget in Targets)
        {
            if (!castSelection.Contains(existingTarget.ActiveEncounterCreatureViewModel))
                toRemove.Add(existingTarget);
        }
        foreach (var existingTarget in toRemove)
        {
            Targets.Remove(existingTarget);
        }
        foreach (var target in Targets)
        {
            target.SelectedDamageVolume = _activeEncounterService.GetDamageVolumeSuggestion(target.ActiveEncounterCreatureViewModel!.Creature, SelectedDamageType);
        }
    }

    private void UpdateCreatures(IList<ObservableActiveEncounterCreature> creatures)
    {
        SelectableCreatures.Clear();
        foreach (var creature in creatures)
            SelectableCreatures.Add(creature);

        var targets = new List<DamageCreatureViewModel>(Targets);

        Targets.Clear();
        foreach (var target in targets)
        {
            if (SelectableCreatures.Contains(target.ActiveEncounterCreatureViewModel))
            {
                Targets.Add(target);
            }
        }
    }
}