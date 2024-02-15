using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
using Windows.ApplicationModel.Search.Core;
using Windows.Devices.WiFi;
using EasyEncounters.Services.Filter;

namespace EasyEncounters.ViewModels;
public partial class CreatureEditViewModel : ObservableRecipientWithValidation, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IDataService _dataService;
    private readonly IList<CreatureAttributeType> _creatureAttributeTypes = Enum.GetValues(typeof(CreatureAttributeType)).Cast<CreatureAttributeType>().ToList();

    //TODO: add an object to wrap these filter properties. No reason to have to repeatedly redeclare them in different views.
    //ideally also have this object handle suggestions and suggestion caches in a cleaner way

    private readonly IFilteringService _filteringService;

    [ObservableProperty]
    private AbilityFilter _abilityFilterValues;

    private List<AbilityViewModel> _spellCache;

    [ObservableProperty]
    private Creature? _creature;

    [ObservableProperty]
    private DamageTypesViewModel? _resists;

    [ObservableProperty]
    private DamageTypesViewModel? _immunities;

    [ObservableProperty]
    private DamageTypesViewModel? _vulnerabilities;

    [ObservableProperty]
    private ConditionTypesViewModel? _conditionImmunities;

    [ObservableProperty]
    private SpellSlotViewModel? _spellSlots;
   
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
    private async Task CommitChanges(object obj)
    {

        if (!HasErrors)
        {
            if (Creature != null)
            {
                Creature.Resistance = Resists?.DamageTypes ?? DamageType.None;
                Creature.Immunity = Immunities?.DamageTypes ?? DamageType.None;
                Creature.Vulnerability = Vulnerabilities?.DamageTypes ?? DamageType.None;
                Creature.ConditionImmunities = ConditionImmunities?.ConditionTypes ?? Condition.None; 
                Creature.Abilities = CreatureAbilities.Select(x => x.Ability).ToList();
            }

            await _dataService.SaveAddAsync(Creature);

            if (!(obj is bool && (bool)obj))
            {
                if (_navigationService.CanGoBack)
                    _navigationService.GoBack();
            }
        }

    }

    [RelayCommand]
    private async Task AddCreatureAbility()
    {
        var ability = new Ability();
        Creature?.Abilities.Add(ability);
        await _dataService.SaveAddAsync(Creature);

        _navigationService.NavigateTo(typeof(AbilityEditViewModel).FullName!, ability);
    }


    public CreatureEditViewModel(INavigationService navigationService, IDataService dataService, IFilteringService filterService)
    {
        _navigationService = navigationService;
        _filteringService = filterService;
        _dataService = dataService;
        _spellCache = new();
        //_searchSuggestions = new();
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

            _spellCache = new List<AbilityViewModel>(Spells);
            AbilityFilterValues = (AbilityFilter)_filteringService.GetFilterValues<AbilityViewModel>();
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
    private async Task EditAbility(object ability)
    {
        if (ability != null && ability is Ability)
        {
            //save current state rather than discarding changes
            await CommitChanges(true);

            _navigationService.NavigateTo(typeof(AbilityEditViewModel).FullName!, ability);
        }
        else if (ability != null && ability is AbilityViewModel)
        {
            await CommitChanges(true);
            _navigationService.NavigateTo(typeof(AbilityEditViewModel).FullName!, ((AbilityViewModel)ability).Ability);
        }
    }

    [RelayCommand]
    private async Task AddAbility()
    {
        var ability = new Ability();
        Spells.Add(new AbilityViewModel(ability));
        await _dataService.SaveAddAsync(ability);
        await EditAbility(ability);
    }

    [RelayCommand]
    private void AddSelectedSpell(object ability)
    {
        if (ability is AbilityViewModel)
        {
            CreatureAbilities.Add((AbilityViewModel)ability);
        }
    }


    [RelayCommand]
    private void AbilityFilter(string text)
    {
        var filtered = _filteringService.Filter(_spellCache, AbilityFilterValues, text);
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
        var filtered = _filteringService.Filter(_spellCache, AbilityFilterValues, text);
        if (String.IsNullOrEmpty(text))
        {
            Spells.Clear();
            foreach (var ability in filtered)
                Spells.Add(ability);
        }
    }



}
