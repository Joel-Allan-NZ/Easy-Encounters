using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.ViewModels;

public partial class DamageInstanceViewModel : ObservableRecipient
{
    private readonly IActiveEncounterService _activeEncounterService;
    private readonly IList<DamageType> _damageTypes = Enum.GetValues(typeof(DamageType)).Cast<DamageType>().ToList();

    [ObservableProperty]
    private int _damageValue;

    [ObservableProperty]
    private DamageType _selectedDamageType;

    public DamageInstanceViewModel(IList<DamageCreatureViewModel> targets, IActiveEncounterService activeEncounterService)
    {
        _activeEncounterService = activeEncounterService;

        Targets.Clear();
        foreach (var target in targets)
        {
            if (target.ActiveEncounterCreatureViewModel != null)
                Targets.Add(new DamageCreatureViewModel(target.ActiveEncounterCreatureViewModel));
        }
        SelectedDamageType = DamageType.None;
    }

    public IList<DamageType> DamageTypes => _damageTypes;
    public ObservableCollection<DamageCreatureViewModel> Targets { get; private set; } = new();

    partial void OnSelectedDamageTypeChanged(DamageType value)
    {
        //add damage receive suggestion logic
        foreach (var target in Targets)
        {
            target.SelectedDamageVolume = _activeEncounterService.GetDamageVolumeSuggestion(target.ActiveEncounterCreatureViewModel.Creature, SelectedDamageType);
        }
    }
}