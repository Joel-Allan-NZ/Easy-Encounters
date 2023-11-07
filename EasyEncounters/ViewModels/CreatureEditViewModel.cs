using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.UI;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Core.Services;
using EasyEncounters.Messages;
using EasyEncounters.Models;
using EasyEncounters.Services;
using Windows.ApplicationModel.Search.Core;
using Windows.Devices.WiFi;

namespace EasyEncounters.ViewModels;
public partial class CreatureEditViewModel : ObservableRecipientWithValidation, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IDataService _dataService;
    private readonly IValidationService _validationService;
    private readonly IList<CreatureAttributeType> _creatureAttributeTypes = Enum.GetValues(typeof(CreatureAttributeType)).Cast<CreatureAttributeType>().ToList();
    private readonly IFilteringService _filteringService;
    private readonly IList<SpellLevel> _spellLevels = Enum.GetValues(typeof(SpellLevel)).Cast<SpellLevel>().ToList();
    private readonly IList<MagicSchool> _magicSchools = Enum.GetValues(typeof(MagicSchool)).Cast<MagicSchool>().ToList();
    private readonly IList<DamageType> _damageTypes = Enum.GetValues(typeof(DamageType)).Cast<DamageType>().ToList();
    private readonly IList<ResolutionType> _resolutionTypes = Enum.GetValues(typeof(ResolutionType)).Cast<ResolutionType>().ToList();
    private readonly IList<ThreeStateBoolean> _concentrationStates = Enum.GetValues(typeof(ThreeStateBoolean)).Cast<ThreeStateBoolean>().ToList();

    public IList<SpellLevel> SpellLevels => _spellLevels;
    public IList<MagicSchool> MagicSchools => _magicSchools;
    public IList<DamageType> DamageTypes => _damageTypes;
    public IList<ResolutionType> ResolutionTypes => _resolutionTypes;
    public IList<ThreeStateBoolean> ConcentrationStates => _concentrationStates;

    private List<AbilityViewModel> _spellCache;

    [ObservableProperty]
    private List<AbilityViewModel> _searchSuggestions;

    [ObservableProperty]
    private SpellLevel _minimumSpellLeveLFilter;

    [ObservableProperty]
    private SpellLevel _maximumSpellLevelFilter;

    [ObservableProperty]
    private MagicSchool _spellSchoolFilterSelected;

    [ObservableProperty]
    private DamageType _damageTypeFilterSelected;

    [ObservableProperty]
    private ResolutionType _resolutionTypeFilterSelected;

    [ObservableProperty]
    private ThreeStateBoolean _concentrationFilterSelected;

    [ObservableProperty]
    private Creature _creature;

    [ObservableProperty]
    private DamageTypesViewModel _resists;

    [ObservableProperty]
    private DamageTypesViewModel _immunities;

    [ObservableProperty]
    private DamageTypesViewModel _vulnerabilities;

    [ObservableProperty]
    private ConditionTypesViewModel _conditionImmunities;

    [ObservableProperty]
    private SpellSlotViewModel _spellSlots;
    

    [Required]
    [CustomValidation(typeof(CreatureEditViewModel), nameof(ValidateLevelCR))]
    public double LevelCR
    {
        get => Creature.LevelOrCR;
        set => TrySetProperty(Creature.LevelOrCR, value, Creature, (v, m) => v.LevelOrCR = m, out IReadOnlyCollection<ValidationResult> errs);
    }

   
    public IList<CreatureAttributeType> StatTypes => _creatureAttributeTypes;

    public ObservableCollection<AbilityViewModel> Spells
    {
        get; set;
    } = new();

    public ObservableCollection<AbilityViewModel> CreatureAbilities
    {
        get; 
        private set;
    } =new();

    [RelayCommand]
    private async void CommitChanges(object obj)
    {
        Creature.Resistance = Resists.DamageTypes;
        Creature.Immunity = Immunities.DamageTypes;
        Creature.Vulnerability = Vulnerabilities.DamageTypes;
        Creature.ConditionImmunities = ConditionImmunities.ConditionTypes;
        Creature.Abilities = CreatureAbilities.Select(x => x.Ability).ToList();

        await _dataService.SaveAddAsync(Creature);

        if (_navigationService.CanGoBack)
            _navigationService.GoBack();
    }

    [RelayCommand]
    private async void AddCreatureAbility()
    {
        var ability = new Ability();
        Creature.Abilities.Add(ability);
        await _dataService.SaveAddAsync(Creature);

        _navigationService.NavigateTo(typeof(AbilityEditViewModel).FullName!, ability);
    }


    public CreatureEditViewModel(INavigationService navigationService, IDataService dataService, IValidationService validationService)
    {
        _navigationService = navigationService;
        _dataService = dataService;
        _validationService = validationService;
    }

    public void OnNavigatedFrom()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
    public async void OnNavigatedTo(object parameter)
    {

        if (parameter is Creature)
        {
            //set
            Creature = (Creature)parameter;
            if (Creature.Hyperlink == null)
                Creature.Hyperlink = "https://www.dndbeyond.com/"; //default safety net.
            Resists = new DamageTypesViewModel(Creature.Resistance);
            Immunities = new DamageTypesViewModel(Creature.Immunity);
            Vulnerabilities = new DamageTypesViewModel(Creature.Vulnerability);
            ConditionImmunities = new ConditionTypesViewModel(Creature.ConditionImmunities);

            CreatureAbilities.Clear();
            foreach(var ability in Creature.Abilities)
            {
                CreatureAbilities.Add(new AbilityViewModel(ability));
            }

            WeakReferenceMessenger.Default.Register<AbilityCRUDRequestMessage>(this, (r, m) =>
            {
                HandleAbilityCRUDRequest(m.Parameter, m.RequestType);
            });

            WeakReferenceMessenger.Default.Register<AddAbilityRequestMessage>(this, (r, m) =>
            {
                CreatureAbilities.Add(m.AbilityViewModel);
            });

            WeakReferenceMessenger.Default.Register<AbilityChangeCommitMessage>(this, (r, m) =>
            {
                //todo:
                AddEditAbility(m.Ability);
            });

            SpellSlots = new SpellSlotViewModel(Creature.SpellSlots);

            var spells = await _dataService.GetAllSpellsAsync();

            foreach(var spell in spells)
            {
                Spells.Add(new AbilityViewModel(spell));
            }
        }
    }

    private async void AddEditAbility(Ability ability)
    {
        //save/update a spell to main Spell list, but just add a regular ability normally.
        //todo: move to service?

        if (ability.SpellLevel != Core.Models.Enums.SpellLevel.NotASpell)
        {
            await _dataService.SaveAddAsync(ability); //todo: make the new spell also appear in this VM's Spells list; currently it won't as we aren't adding it or refreshing the list.
        }

        var existingAbility = CreatureAbilities.FirstOrDefault(x => x.Ability == ability);

        if (existingAbility == null)
        {
            CreatureAbilities.Add(new AbilityViewModel(ability));
        }
    }

    private void HandleAbilityCRUDRequest(AbilityViewModel ability, CRUDRequestType requestType)
    {
        if(requestType == CRUDRequestType.Edit)
        {
            _navigationService.NavigateTo(typeof(AbilityEditViewModel).FullName!, ability.Ability);
        }
        else if(requestType == CRUDRequestType.Delete)
        {
            CreatureAbilities.Remove(ability);
        }
    }

    [RelayCommand]
    private async void EditAbility(object ability)
    {
        if (ability != null && ability is Ability)
        {
            _navigationService.NavigateTo(typeof(AbilityEditViewModel).FullName!, ability);
        }
        else if (ability != null && ability is AbilityViewModel)
        {
            _navigationService.NavigateTo(typeof(AbilityEditViewModel).FullName!, ((AbilityViewModel)ability).Ability);
        }
    }

    [RelayCommand]
    private async void AddAbility()
    {
        var ability = new Ability();
        Spells.Add(new AbilityViewModel(ability));
        await _dataService.SaveAddAsync(ability);
        EditAbility(ability);
    }

    public static ValidationResult ValidateLevelCR(double levelCR, ValidationContext context)
    {
        //todo: move validation to a service.
        var instance = (CreatureEditViewModel)context.ObjectInstance;

        bool valid = instance._validationService.Validate(instance, levelCR, "LevelCR");
        if(valid)
            return ValidationResult.Success;

        return new("Not a valid Level or Challenge Rating");
    }


    [RelayCommand]
    private void AbilityFilter(string text)
    {
        List<FilterCriteria<AbilityViewModel>> criteria = new()
        {
            new FilterCriteria<AbilityViewModel>(x => x.Ability.SpellLevel, MinimumSpellLeveLFilter, MaximumSpellLevelFilter),
            new FilterCriteria<AbilityViewModel>(x => x.Ability.Name, text),
        };
        if (ConcentrationFilterSelected == ThreeStateBoolean.False)
            criteria.Add(new FilterCriteria<AbilityViewModel>(x => x.Ability.Concentration, false, false));
        if (ConcentrationFilterSelected == ThreeStateBoolean.True)
            criteria.Add(new FilterCriteria<AbilityViewModel>(x => x.Ability.Concentration, true, true));
        if (ResolutionTypeFilterSelected != ResolutionType.Undefined)
            criteria.Add(new FilterCriteria<AbilityViewModel>(x => x.Ability.Resolution, ResolutionTypeFilterSelected, ResolutionTypeFilterSelected));
        if (SpellSchoolFilterSelected != MagicSchool.None)
            criteria.Add(new FilterCriteria<AbilityViewModel>(x => x.Ability.MagicSchool, SpellSchoolFilterSelected, SpellSchoolFilterSelected));
        if (DamageTypeFilterSelected != DamageType.Untyped)
            criteria.Add(new FilterCriteria<AbilityViewModel>(x => x.Ability.DamageTypes, DamageTypeFilterSelected, DamageTypeFilterSelected));

        var filtered = _filteringService.Filter(_spellCache, criteria);
        Spells.Clear();
        foreach (var ability in filtered)
            Spells.Add(ability);
    }

    [RelayCommand]
    private void DataGridSort(DataGridColumnEventArgs e)
    {
        //OnSorting(e);
        if (e.Column.Tag.ToString() == "AbilityName")
        {
            SortByPredicate(Spells, x => x.Ability.Name, e.Column.SortDirection == DataGridSortDirection.Ascending);
            //if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
            //    SortAbilitiesByName(false);
            //else
            //    SortAbilitiesByName(true);
        }
        else if (e.Column.Tag.ToString() == "AbilityLevel")
        {
            SortByPredicate(Spells, x => x.Ability.SpellLevel, e.Column.SortDirection == DataGridSortDirection.Ascending);
        }
        else if (e.Column.Tag.ToString() == "AbilityDamageType")
        {
            SortByPredicate(Spells, x => x.Ability.DamageTypes, e.Column.SortDirection == DataGridSortDirection.Ascending);
        }
        else if (e.Column.Tag.ToString() == "AbilityResolutionType")
        {
            SortByPredicate(Spells, x => x.Ability.Resolution, e.Column.SortDirection == DataGridSortDirection.Ascending);
        }
        else if (e.Column.Tag.ToString() == "AbilityConcentration")
        {
            SortByPredicate(Spells, x => x.Ability.Concentration, e.Column.SortDirection == DataGridSortDirection.Ascending);
        }
        else if (e.Column.Tag.ToString() == "AbilityResolutionStat")
        {
            SortByPredicate(Spells, x => x.Ability.SaveType, e.Column.SortDirection == DataGridSortDirection.Ascending);
        }
        else if (e.Column.Tag.ToString() == "AbilitySchool")
        {
            SortByPredicate(Spells, x => x.Ability.MagicSchool, e.Column.SortDirection == DataGridSortDirection.Ascending);
        }

        if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
        {
            e.Column.SortDirection = DataGridSortDirection.Ascending;
        }
        else
        {
            e.Column.SortDirection = DataGridSortDirection.Descending;
        }

    }

    private void SortByPredicate<T, U>(ObservableCollection<T> collection, Func<T, U> expression, bool ascending)
    {
        IEnumerable<T> tmp = (ascending) ? collection.OrderBy(expression).ToList() : collection.OrderByDescending(expression).ToList();

        collection.Clear();
        foreach (var item in tmp)
            collection.Add(item);
    }


    [RelayCommand]
    private void SearchTextChange(string text)
    {
        if (String.IsNullOrEmpty(text))
        {
            var filtered = _filteringService.Filter(_spellCache, x => x.Ability.SpellLevel, MinimumSpellLeveLFilter, MaximumSpellLevelFilter);
            Spells.Clear();
            foreach (var ability in filtered)
                Spells.Add(ability);
        }
        SearchSuggestions = _filteringService.Filter(Spells, x => x.Ability.Name, text).ToList();
    }



}
