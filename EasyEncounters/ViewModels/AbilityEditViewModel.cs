using System;
using System.Collections.Generic;
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
using EasyEncounters.Models;

namespace EasyEncounters.ViewModels;
public partial class AbilityEditViewModel : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IDataService _dataService;
    private readonly IAbilityService _abilityService;
    private Ability? _ability;

    //todo: find better way to do this.
    private readonly IList<SpellLevel> _spellLevels = Enum.GetValues(typeof(SpellLevel)).Cast<SpellLevel>().ToList();
    private readonly IList<CreatureAttributeType> _creatureAttributeTypes = Enum.GetValues(typeof(CreatureAttributeType)).Cast<CreatureAttributeType>().ToList();
    private readonly IList<TargetAreaType> _targetAreaTypes = Enum.GetValues(typeof(TargetAreaType)).Cast<TargetAreaType>().ToList();
    private readonly IList<DamageType> _damageTypes = Enum.GetValues(typeof(DamageType)).Cast<DamageType>().ToList();
    private readonly IList<ResolutionType> _resolutionTypes = Enum.GetValues(typeof(ResolutionType)).Cast<ResolutionType>().ToList();
    private readonly IList<ActionSpeed> _actionSpeeds = Enum.GetValues(typeof(ActionSpeed)).Cast<ActionSpeed>().ToList();
    private readonly IList<ActionRangeType> _actionRanges = Enum.GetValues(typeof(ActionRangeType)).Cast<ActionRangeType>().ToList();
    private readonly IList<MagicSchool> _magicSchools = Enum.GetValues(typeof(MagicSchool)).Cast<MagicSchool>().ToList();
    private readonly IList<TimeDuration> _timeDurations = Enum.GetValues(typeof(TimeDuration)).Cast<TimeDuration>().ToList();


    [ObservableProperty]
    private SpellCastComponent _spellCastComponents;

    [ObservableProperty]
    private bool _spellCastVerbal;

    [ObservableProperty]
    private bool _spellCastMaterial;

    [ObservableProperty]
    private bool _spellCastSomatic;

    partial void OnSpellCastVerbalChanged(bool value) => AddRemoveSpellCastComponent(value, SpellCastComponent.Verbal);

    partial void OnSpellCastMaterialChanged(bool value) => AddRemoveSpellCastComponent(value, SpellCastComponent.Material);

    partial void OnSpellCastSomaticChanged(bool value) => AddRemoveSpellCastComponent(value, SpellCastComponent.Somatic);


    private void AddRemoveSpellCastComponent(bool value, SpellCastComponent castComponent)
    {
        if (value)
            SpellCastComponents |= castComponent;
        else
            SpellCastComponents &= ~castComponent;
    }

    public IList<SpellLevel> SpellLevels => _spellLevels;
    public IList<CreatureAttributeType> StatTypes => _creatureAttributeTypes;
    public IList<TargetAreaType> TargetAreaTypes => _targetAreaTypes;
    public IList<DamageType> DamageTypes => _damageTypes;
    public IList<ResolutionType> ResolutionTypes => _resolutionTypes;
    public IList<ActionSpeed> ActionSpeeds => _actionSpeeds;
    public IList<ActionRangeType> ActionRangeTypes => _actionRanges;
    public IList<MagicSchool> MagicSchools => _magicSchools;
    public IList<TimeDuration> TimeDurations => _timeDurations;


    [ObservableProperty]
    private SpellLevel _selectedSpellLevel;

    [ObservableProperty]
    private DamageType _selectedDamageType;

    partial void OnSelectedDamageTypeChanged(DamageType value)
    {
        if(ObservableAbility != null)
            ObservableAbility.DamageType = value;
    } 

    [ObservableProperty]
    private ObservableAbility? _observableAbility;

    [RelayCommand]
    private async Task CommitChanges(object obj)
    {
        if (ObservableAbility == null || _ability == null)
        {
            return;
        }
        ObservableAbility.SpellCastComponents = SpellCastComponents;
        if(ObservableAbility.SpellLevel != SpellLevel.NotASpell)
            await _dataService.SaveAddAsync(_ability);

        WeakReferenceMessenger.Default.Send(new AbilityChangeCommitMessage(_ability));

        if (_navigationService.CanGoBack)
            _navigationService.GoBack();
    }

    public AbilityEditViewModel(IAbilityService abilityService, IDataService dataService, INavigationService navigationService)
    {

        _abilityService = abilityService;
        _dataService = dataService;
        _navigationService = navigationService;
      

    }
    public void OnNavigatedFrom()
    {

    }
    public void OnNavigatedTo(object parameter)
    {
        if(parameter != null && parameter is Ability)
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
}
