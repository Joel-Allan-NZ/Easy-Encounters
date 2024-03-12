using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Messages;
using EasyEncounters.Models;

namespace EasyEncounters.ViewModels;

public partial class AbilityEditViewModel : ObservableRecipient, INavigationAware
{
    private readonly IAbilityService _abilityService;
    private readonly IList<ActionRangeType> _actionRanges = Enum.GetValues(typeof(ActionRangeType)).Cast<ActionRangeType>().ToList();
    private readonly IList<ActionSpeed> _actionSpeeds = Enum.GetValues(typeof(ActionSpeed)).Cast<ActionSpeed>().ToList();
    private readonly IList<CreatureAttributeType> _creatureAttributeTypes = Enum.GetValues(typeof(CreatureAttributeType)).Cast<CreatureAttributeType>().ToList();
    private readonly IList<DamageType> _damageTypes = Enum.GetValues(typeof(DamageType)).Cast<DamageType>().ToList();
    private readonly IDataService _dataService;
    private readonly IList<MagicSchool> _magicSchools = Enum.GetValues(typeof(MagicSchool)).Cast<MagicSchool>().ToList();
    private readonly INavigationService _navigationService;
    private readonly IList<ResolutionType> _resolutionTypes = Enum.GetValues(typeof(ResolutionType)).Cast<ResolutionType>().ToList();

    //todo: find better way to do this.
    private readonly IList<SpellLevel> _spellLevels = Enum.GetValues(typeof(SpellLevel)).Cast<SpellLevel>().ToList();

    private readonly IList<TargetAreaType> _targetAreaTypes = Enum.GetValues(typeof(TargetAreaType)).Cast<TargetAreaType>().ToList();
    private readonly IList<TimeDuration> _timeDurations = Enum.GetValues(typeof(TimeDuration)).Cast<TimeDuration>().ToList();
    private Ability? _ability;

    [ObservableProperty]
    private ObservableAbility? _observableAbility;

    [ObservableProperty]
    private DamageType _selectedDamageType;

    [ObservableProperty]
    private SpellLevel _selectedSpellLevel;

    [ObservableProperty]
    private SpellCastComponent _spellCastComponents;

    [ObservableProperty]
    private bool _spellCastMaterial;

    [ObservableProperty]
    private bool _spellCastSomatic;

    [ObservableProperty]
    private bool _spellCastVerbal;

    public AbilityEditViewModel(IAbilityService abilityService, IDataService dataService, INavigationService navigationService)
    {
        _abilityService = abilityService;
        _dataService = dataService;
        _navigationService = navigationService;
    }

    public IList<ActionRangeType> ActionRangeTypes => _actionRanges;

    public IList<ActionSpeed> ActionSpeeds => _actionSpeeds;

    public IList<DamageType> DamageTypes => _damageTypes;

    public IList<MagicSchool> MagicSchools => _magicSchools;

    public IList<ResolutionType> ResolutionTypes => _resolutionTypes;

    public IList<SpellLevel> SpellLevels => _spellLevels;

    public IList<CreatureAttributeType> StatTypes => _creatureAttributeTypes;

    public IList<TargetAreaType> TargetAreaTypes => _targetAreaTypes;

    public IList<TimeDuration> TimeDurations => _timeDurations;

    public void OnNavigatedFrom()
    {
    }

    public void OnNavigatedTo(object parameter)
    {
        if (parameter != null && parameter is Ability)
        {
            _ability = (Ability)parameter;
            ObservableAbility = new ObservableAbility(_ability);

            SelectedDamageType = ObservableAbility.DamageType;

            SpellCastComponents = (SpellCastComponent)((int)ObservableAbility.SpellCastComponents);

            SpellCastMaterial = SpellCastComponents.HasFlag(SpellCastComponent.Material);
            SpellCastVerbal = SpellCastComponents.HasFlag(SpellCastComponent.Verbal);
            SpellCastSomatic = SpellCastComponents.HasFlag(SpellCastComponent.Somatic);
        }
    }

    private void AddRemoveSpellCastComponent(bool value, SpellCastComponent castComponent)
    {
        if (value)
            SpellCastComponents |= castComponent;
        else
            SpellCastComponents &= ~castComponent;
    }

    [RelayCommand]
    private async Task CommitChanges(object obj)
    {
        if (ObservableAbility == null || _ability == null)
        {
            return;
        }
        ObservableAbility.SpellCastComponents = SpellCastComponents;
        if (ObservableAbility.SpellLevel != SpellLevel.NotASpell)
            await _dataService.SaveAddAsync(_ability);

        WeakReferenceMessenger.Default.Send(new AbilityCRUDRequestMessage(_ability, CRUDRequestType.Edit));

        if (_navigationService.CanGoBack)
            _navigationService.GoBack();
    }

    partial void OnSelectedDamageTypeChanged(DamageType value)
    {
        if (ObservableAbility != null)
            ObservableAbility.DamageType = value;
    }

    partial void OnSpellCastMaterialChanged(bool value) => AddRemoveSpellCastComponent(value, SpellCastComponent.Material);

    partial void OnSpellCastSomaticChanged(bool value) => AddRemoveSpellCastComponent(value, SpellCastComponent.Somatic);

    partial void OnSpellCastVerbalChanged(bool value) => AddRemoveSpellCastComponent(value, SpellCastComponent.Verbal);
}