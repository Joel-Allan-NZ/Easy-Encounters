//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;
//using EasyEncounters.Contracts.Services;
//using EasyEncounters.Contracts.ViewModels;
//using EasyEncounters.Core.Contracts.Services;
//using EasyEncounters.Core.Models;
//using EasyEncounters.Core.Models.Enums;
//using SQLitePCL;

//namespace EasyEncounters.ViewModels;
//public partial class CreatureAbilityViewModel : ObservableRecipient, INavigationAware
//{
//    private readonly INavigationService _navigationService;
//    private readonly IDataService _dataService;

//    private Creature? _creature;
//    //todo: find better way to do this.
//    private IList<SpellLevel> _spellLevels = Enum.GetValues(typeof(SpellLevel)).Cast<SpellLevel>().ToList();
//    private IList<CreatureAttributeType> _creatureAttributeTypes = Enum.GetValues(typeof(CreatureAttributeType)).Cast<CreatureAttributeType>().ToList();
//    private IList<TargetAreaType> _targetAreaTypes = Enum.GetValues(typeof(TargetAreaType)).Cast<TargetAreaType>().ToList();
//    private IList<DamageType> _damageTypes = Enum.GetValues(typeof(DamageType)).Cast<DamageType>().ToList();
//    private IList<ResolutionType> _resolutionTypes = Enum.GetValues(typeof(ResolutionType)).Cast<ResolutionType>().ToList();
//    private IList<ActionSpeed> _actionSpeeds = Enum.GetValues(typeof(ActionSpeed)).Cast<ActionSpeed>().ToList();

//    private SpellCastComponent _spellCastComponent = SpellCastComponent.None;

//    [ObservableProperty]
//    private bool _spellCastVerbal;

//    [ObservableProperty]
//    private bool _spellCastMaterial;

//    [ObservableProperty]
//    private bool _spellCastSomatic;

//    partial void OnSpellCastVerbalChanged(bool value) => AddRemoveSpellCastComponent(value, SpellCastComponent.Verbal);

//    partial void OnSpellCastMaterialChanged(bool value) => AddRemoveSpellCastComponent(value, SpellCastComponent.Somatic);

//    partial void OnSpellCastSomaticChanged(bool value) => AddRemoveSpellCastComponent(value, SpellCastComponent.Material);


//    private void AddRemoveSpellCastComponent(bool value, SpellCastComponent castComponent)
//    {
//        if (value)
//            _spellCastComponent |= castComponent;
//        else
//            _spellCastComponent &= ~castComponent;
//    }

//    public IList<SpellLevel> SpellLevels => _spellLevels;
//    public IList<CreatureAttributeType> StatTypes => _creatureAttributeTypes;
//    public IList<TargetAreaType> TargetAreaTypes => _targetAreaTypes;
//    public IList<DamageType> DamageTypes => _damageTypes;
//    public IList<ResolutionType> ResolutionTypes => _resolutionTypes;
//    public IList<ActionSpeed> ActionSpeeds => _actionSpeeds;


//    [ObservableProperty]
//    private SpellLevel _selectedSpellLevel;



//    [ObservableProperty]
//    private Ability? _ability;

//    [RelayCommand]
//    private async void CommitChanges(object obj)
//    {
//        await _dataService.SaveAddAsync(_creature);
//        if (_navigationService.CanGoBack)
//            _navigationService.GoBack();

//        //todo: destroy this class. Instead, just have the creatureedit open an abilityeditviewmodel, and pass back a message when finished. 
//        // Creature can add that ability, and also at the same time call SaveAddAsync to update saved stuff
//        // can also use this to directly add a spell from creature editing rather than specifically adding a spell - just check if it it's a spell,
//        // and if it is, add it to spells list and call a SaveAddAsync for that passed-back spell specifically (to add it to spells list)
//    }

//    public CreatureAbilityViewModel(INavigationService navigationService, IDataService dataService)
//    {
//        _navigationService = navigationService;
//        _dataService = dataService;
//    }

//    public void OnNavigatedFrom()
//    {

//    }

//    public void OnNavigatedTo(object parameter)
//    {
//        if (parameter is Tuple<Creature, Ability>) //yikes.
//        {
//            var tup = (Tuple<Creature, Ability>)parameter;

//            _creature = tup.Item1;
//            Ability = tup.Item2;

            
//            SpellCastVerbal = Ability.CastingComponents.HasFlag(SpellCastComponent.Verbal);
//            SpellCastMaterial = Ability.CastingComponents.HasFlag(SpellCastComponent.Material);
//            SpellCastSomatic = Ability.CastingComponents.HasFlag(SpellCastComponent.Somatic);

//        }
//    }
//}
