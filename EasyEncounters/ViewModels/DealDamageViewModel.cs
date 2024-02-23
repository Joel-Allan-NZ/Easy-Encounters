using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;

namespace EasyEncounters.ViewModels;

public partial class DealDamageViewModel : ObservableRecipient, INavigationAware
{
    private readonly IActiveEncounterService _activeEncounterService;

    private readonly INavigationService _navigationService;

    private ActiveEncounter? _activeEncounter;

    [ObservableProperty]
    private ActiveEncounterCreatureViewModel? _sourceCreature;

    //public ObservableCollection<ActiveEncounterCreatureViewModel> Targets = new();
    private IList<DamageCreatureViewModel> _targets;

    public DealDamageViewModel(IActiveEncounterService activeEncounterService, INavigationService navigationService)
    {
        _navigationService = navigationService;
        _activeEncounterService = activeEncounterService;
        _targets = new List<DamageCreatureViewModel>();
    }

    public ObservableCollection<DamageInstanceViewModel> DamageInstances
    {
        get; private set;
    } = new();

    public void OnNavigatedFrom()
    {
    }

    public void OnNavigatedTo(object parameter)
    {
        if (parameter is DealDamageTargetting && parameter != null)
        {
            //set properties
            var targetting = (DealDamageTargetting)parameter;

            SourceCreature = targetting.Source;

            _activeEncounter = targetting.Encounter;
            _targets = targetting.Targets.Select(x => new DamageCreatureViewModel(x)).ToList();

            AddDamage();
        }
    }

    [RelayCommand]
    private void AddDamage()
    {
        DamageInstances.Add(new DamageInstanceViewModel(_targets, _activeEncounterService));
    }

    [RelayCommand]
    private void DealDamage()
    {
        var instances = GetInstancesOfDamage();
        foreach (var instance in instances)
            _activeEncounterService.DealDamageAsync(_activeEncounter, instance);

        _navigationService.NavigateTo(typeof(RunEncounterViewModel).FullName!, ignoreNavigation: true);
    }

    private List<DamageInstance> GetInstancesOfDamage()
    {
        List<DamageInstance> damageInstances = new List<DamageInstance>();

        foreach (var damage in DamageInstances)
        {
            foreach (var target in damage.Targets)
            {
                damageInstances.Add(new DamageInstance(target.ActiveEncounterCreatureViewModel.Creature, SourceCreature?.Creature, damage.SelectedDamageType, target.SelectedDamageVolume, damage.DamageValue));
            }
        }
        return damageInstances;
    }
}