//using System.Collections.ObjectModel;
//using ABI.System.Collections.Generic;
//using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;
//using CommunityToolkit.Mvvm.Messaging;
//using CommunityToolkit.WinUI.UI.Controls;
//using EasyEncounters.Contracts.Services;
//using EasyEncounters.Contracts.ViewModels;
//using EasyEncounters.Core.Contracts.Services;
//using EasyEncounters.Core.Models;
//using EasyEncounters.Core.Models.Enums;
//using EasyEncounters.Messages;
//using EasyEncounters.Models;
//using EasyEncounters.Services.Filter;
//using Microsoft.UI.Composition;

//namespace EasyEncounters.ViewModels;

//public partial class CreatureEditViewModel : ObservableRecipientWithValidation, INavigationAware
//{
//    private readonly IList<CreatureAttributeType> _creatureAttributeTypes = Enum.GetValues(typeof(CreatureAttributeType)).Cast<CreatureAttributeType>().ToList();
//    private readonly IList<CreatureSizeClass> _creatureSizeClasses = Enum.GetValues(typeof(CreatureSizeClass)).Cast<CreatureSizeClass>().ToList();
//    private readonly IList<CreatureAlignment> _creatureAlignments = Enum.GetValues(typeof(CreatureAlignment)).Cast<CreatureAlignment>().ToList();
//    private readonly IList<CreatureType> _creatureTypes = Enum.GetValues(typeof(CreatureType)).Cast<CreatureType>().ToList();

//    private readonly IDataService _dataService;
//    private readonly IFilteringService _filteringService;
//    private readonly INavigationService _navigationService;

//    //TODO: add an object to wrap these filter properties. No reason to have to repeatedly redeclare them in different views.
//    //ideally also have this object handle suggestions and suggestion caches in a cleaner way
//    [ObservableProperty]
//    private AbilityFilter _abilityFilterValues;

//    [ObservableProperty]
//    private ConditionsHelper? _conditionImmunities;

//    [ObservableProperty]
//    private Creature? _creature;

//    [ObservableProperty]
//    private DamageTypesViewModel? _immunities;

//    [ObservableProperty]
//    private DamageTypesViewModel? _resists;

//    private List<Ability> _spellCache;

//    [ObservableProperty]
//    private SpellSlotViewModel? _spellSlots;

//    [ObservableProperty]
//    private DamageTypesViewModel? _vulnerabilities;

//    public CreatureEditViewModel(INavigationService navigationService, IDataService dataService, IFilteringService filterService)
//    {
//        _navigationService = navigationService;
//        _filteringService = filterService;
//        _dataService = dataService;
//        _spellCache = new();
//        _abilityFilterValues = (AbilityFilter)_filteringService.GetFilterValues<Ability>();
//        //_searchSuggestions = new();
//    }

//    public ObservableCollection<Ability> CreatureAbilities
//    {
//        get;
//        private set;
//    } = new();

//    public ObservableCollection<Ability> Spells
//    {
//        get; set;
//    } = new();

//    public IList<CreatureAttributeType> StatTypes => _creatureAttributeTypes;
//    public IList<CreatureType> CreatureTypes => _creatureTypes;
//    public IList<CreatureAlignment> CreatureAlignments => _creatureAlignments;
//    public IList<CreatureSizeClass> CreatureSizeClasses => _creatureSizeClasses;

//    public void OnNavigatedFrom()
//    {
//        WeakReferenceMessenger.Default.UnregisterAll(this);
//    }

//    public async void OnNavigatedTo(object parameter)
//    {
//        if (parameter is Creature)
//        {
//            //set
//            Creature = (Creature)parameter;
//            if (Creature.Hyperlink == null)
//                Creature.Hyperlink = "https://www.dndbeyond.com/"; //default safety net.
//            Resists = new DamageTypesViewModel(Creature.Resistance);
//            Immunities = new DamageTypesViewModel(Creature.Immunity);
//            Vulnerabilities = new DamageTypesViewModel(Creature.Vulnerability);
//            ConditionImmunities = new ConditionsHelper(Creature.ConditionImmunities);

//            CreatureAbilities.Clear();
//            foreach (var ability in Creature.Abilities)
//            {
//                CreatureAbilities.Add(ability);
//            }

//            WeakReferenceMessenger.Default.Register<AbilityCRUDRequestMessage>(this, (r, m) =>
//            {
//                HandleAbilityCRUDRequest(m.Parameter, m.RequestType);
//            });

//            SpellSlots = new SpellSlotViewModel(Creature.SpellSlots);

//            var spells = await _dataService.GetAllSpellsAsync();

//            foreach (var spell in spells)
//            {
//                Spells.Add(spell);
//            }

//            _spellCache = new List<Ability>(Spells);
//            AbilityFilterValues = (AbilityFilter)_filteringService.GetFilterValues<Ability>();
//        }
//    }

//    [RelayCommand]
//    private void AbilityFilter(string text)
//    {
//        var filtered = _filteringService.Filter(_spellCache, AbilityFilterValues, text);
//        Spells.Clear();
//        foreach (var ability in filtered)
//            Spells.Add(ability);
//    }

//    [RelayCommand]
//    private async Task AddAbility()
//    {
//        var ability = new Ability();
//        Spells.Add(ability);
//        await _dataService.SaveAddAsync(ability);
//        await EditAbility(ability);
//    }

//    [RelayCommand]
//    private async Task AddCreatureAbility()
//    {
//        var ability = new Ability();
//        Creature?.Abilities.Add(ability);
//        await _dataService.SaveAddAsync(Creature);

//        _navigationService.NavigateTo(typeof(AbilityEditViewModel).FullName!, ability);
//    }

//    private async void AddEditAbility(Ability ability)
//    {
//        //save/update a spell to main Spell list, but just add a regular ability normally.
//        //todo: move to service?

//        if (ability.SpellLevel != Core.Models.Enums.SpellLevel.NotASpell)
//        {
//            await _dataService.SaveAddAsync(ability); //todo: make the new spell also appear in this VM's Spells list; currently it won't as we aren't adding it or refreshing the list.
//        }

//        var existingAbility = CreatureAbilities.FirstOrDefault(x => x == ability);

//        if (existingAbility == null)
//        {
//            CreatureAbilities.Add(ability);
//        }
//    }

//    [RelayCommand]
//    private void AddSelectedSpell(object ability)
//    {
//        if (ability is Ability)
//        {
//            CreatureAbilities.Add((Ability)ability);
//        }
//    }

//    [RelayCommand]
//    private async Task CommitChanges(object obj)
//    {
//        if (!HasErrors)
//        {
//            if (Creature != null)
//            {
//                Creature.Resistance = Resists?.DamageTypes ?? DamageType.None;
//                Creature.Immunity = Immunities?.DamageTypes ?? DamageType.None;
//                Creature.Vulnerability = Vulnerabilities?.DamageTypes ?? DamageType.None;
//                Creature.ConditionImmunities = ConditionImmunities?.ConditionTypes ?? Condition.None;
//                Creature.Abilities = CreatureAbilities.Select(x => x).ToList();
//            }

//            await _dataService.SaveAddAsync(Creature);

//            if (!(obj is bool && (bool)obj))
//            {
//                if (_navigationService.CanGoBack)
//                    _navigationService.GoBack();
//            }
//        }
//    }

//    [RelayCommand]
//    private void DataGridSort(DataGridColumnEventArgs e)
//    {
//        //OnSorting(e);
//        if (e.Column.Tag.ToString() == "AbilityName")
//        {
//            SortByPredicate(Spells, x => x.Name, e.Column.SortDirection == DataGridSortDirection.Ascending);
//        }
//        else if (e.Column.Tag.ToString() == "AbilityLevel")
//        {
//            SortByPredicate(Spells, x => x.SpellLevel, e.Column.SortDirection == DataGridSortDirection.Ascending);
//        }
//        else if (e.Column.Tag.ToString() == "AbilityDamageType")
//        {
//            SortByPredicate(Spells, x => x.DamageTypes, e.Column.SortDirection == DataGridSortDirection.Ascending);
//        }
//        else if (e.Column.Tag.ToString() == "AbilityResolutionType")
//        {
//            SortByPredicate(Spells, x => x.Resolution, e.Column.SortDirection == DataGridSortDirection.Ascending);
//        }
//        else if (e.Column.Tag.ToString() == "AbilityConcentration")
//        {
//            SortByPredicate(Spells, x => x.Concentration, e.Column.SortDirection == DataGridSortDirection.Ascending);
//        }
//        else if (e.Column.Tag.ToString() == "AbilityResolutionStat")
//        {
//            SortByPredicate(Spells, x => x.SaveType, e.Column.SortDirection == DataGridSortDirection.Ascending);
//        }
//        else if (e.Column.Tag.ToString() == "AbilitySchool")
//        {
//            SortByPredicate(Spells, x => x.MagicSchool, e.Column.SortDirection == DataGridSortDirection.Ascending);
//        }

//        if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
//        {
//            e.Column.SortDirection = DataGridSortDirection.Ascending;
//        }
//        else
//        {
//            e.Column.SortDirection = DataGridSortDirection.Descending;
//        }
//    }

//    [RelayCommand]
//    private async Task EditAbility(Ability ability)
//    {
//        if (ability != null && ability is Ability)
//        {
//            //save current state rather than discarding changes
//            await CommitChanges(true);

//            _navigationService.NavigateTo(typeof(AbilityEditViewModel).FullName!, ability);
//        }
//    }

//    [RelayCommand]
//    private void DeleteAbility(Ability ability)
//    {
//        CreatureAbilities.Remove(ability);
//    }

//    private void HandleAbilityCRUDRequest(Ability ability, CRUDRequestType requestType)
//    {
//        if (requestType == CRUDRequestType.Edit)
//        {
//            _navigationService.NavigateTo(typeof(AbilityEditViewModel).FullName!, ability);
//        }
//        else if (requestType == CRUDRequestType.Delete)
//        {
//            CreatureAbilities.Remove(ability);
//        }
//        else if(requestType == CRUDRequestType.Add)
//        {
//            CreatureAbilities.Add(ability);
//        }
//    }

//    [RelayCommand]
//    private void SearchTextChange(string text)
//    {
//        var filtered = _filteringService.Filter(_spellCache, AbilityFilterValues, text);
//        if (String.IsNullOrEmpty(text))
//        {
//            Spells.Clear();
//            foreach (var ability in filtered)
//                Spells.Add(ability);
//        }
//    }

//    private void SortByPredicate<T, U>(ObservableCollection<T> collection, Func<T, U> expression, bool ascending)
//    {
//        IEnumerable<T> tmp = (ascending) ? collection.OrderBy(expression).ToList() : collection.OrderByDescending(expression).ToList();

//        collection.Clear();
//        foreach (var item in tmp)
//            collection.Add(item);
//    }
//}