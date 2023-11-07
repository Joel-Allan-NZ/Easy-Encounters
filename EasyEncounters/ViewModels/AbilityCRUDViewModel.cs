using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
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
using EasyEncounters.Messages;
using EasyEncounters.Models;
using EasyEncounters.Services;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Windows.Media.Playback;

namespace EasyEncounters.ViewModels;
public partial class AbilityCRUDViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService _dataService;
    private readonly INavigationService _navigationService;
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

    private List<AbilityViewModel> _abilityCache;

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






    //public event EventHandler<DataGridColumnEventArgs> Sorting;
    //protected virtual void OnSorting(DataGridColumnEventArgs e)
    //{
    //    EventHandler<DataGridColumnEventArgs> handler = Sorting;
    //    if (handler != null)
    //        handler(this, e);
    //}

    [RelayCommand]
    private void SearchTextChange(string text)
    {
        if (String.IsNullOrEmpty(text))
        {
            var filtered = _filteringService.Filter(_abilityCache, x => x.Ability.SpellLevel, MinimumSpellLeveLFilter, MaximumSpellLevelFilter);
            Abilities.Clear();
            foreach(var ability in filtered)
                Abilities.Add(ability);
        }
        SearchSuggestions = _filteringService.Filter(Abilities, x => x.Ability.Name, text).ToList();
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

        var filtered = _filteringService.Filter(_abilityCache, criteria);
        Abilities.Clear();
        foreach (var ability in filtered)
            Abilities.Add(ability);
    }

    [RelayCommand]
    private void DataGridSort(DataGridColumnEventArgs e)
    {
        //OnSorting(e);
        if (e.Column.Tag.ToString() == "AbilityName")
        {
            SortByPredicate(Abilities, x => x.Ability.Name, e.Column.SortDirection == DataGridSortDirection.Ascending);
            //if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
            //    SortAbilitiesByName(false);
            //else
            //    SortAbilitiesByName(true);
        }
        else if (e.Column.Tag.ToString() == "AbilityLevel")
        {
                SortByPredicate(Abilities, x => x.Ability.SpellLevel, e.Column.SortDirection == DataGridSortDirection.Ascending);
        }
        else if(e.Column.Tag.ToString() == "AbilityDamageType")
        {
            SortByPredicate(Abilities, x => x.Ability.DamageTypes, e.Column.SortDirection == DataGridSortDirection.Ascending);
        }
        else if(e.Column.Tag.ToString() == "AbilityResolutionType")
        {
            SortByPredicate(Abilities, x => x.Ability.Resolution, e.Column.SortDirection == DataGridSortDirection.Ascending);
        }
        else if(e.Column.Tag.ToString() == "AbilityConcentration")
        {
            SortByPredicate(Abilities, x => x.Ability.Concentration, e.Column.SortDirection == DataGridSortDirection.Ascending);
        }
        else if (e.Column.Tag.ToString() == "AbilityResolutionStat")
        {
            SortByPredicate(Abilities, x => x.Ability.SaveType, e.Column.SortDirection == DataGridSortDirection.Ascending);
        }
        else if (e.Column.Tag.ToString() == "AbilitySchool")
        {
            SortByPredicate(Abilities, x => x.Ability.MagicSchool, e.Column.SortDirection == DataGridSortDirection.Ascending);
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
        foreach(var item in tmp)
            collection.Add(item);
    }


    public ObservableCollection<AbilityViewModel> Abilities
    {
        get; set;
    } = new();

    [RelayCommand]
    private async void DeleteAbility(object parameter)
    {
        if (parameter != null && parameter is Ability)
        {
            var toDelete = (Ability)parameter;
            Abilities.Remove(Abilities.First(x => x.Ability == toDelete));
            await _dataService.DeleteAsync(toDelete);
        }
    }

    [RelayCommand]
    private async void EditAbility(object ability)
    {
        if(ability != null && ability is Ability)
        {
            _navigationService.NavigateTo(typeof(AbilityEditViewModel).FullName!, ability);
        }
        else if (ability != null && ability is AbilityViewModel)
        {
            _navigationService.NavigateTo(typeof(AbilityEditViewModel).FullName!, ((AbilityViewModel)ability).Ability);
        }
    }

    [RelayCommand]
    private async void CopyAbility(object ability)
    {
        if(ability != null && ability is Ability)
        {
            var copied = await _dataService.CopyAsync(ability as Ability);
            Abilities.Add(new AbilityViewModel(copied));
        }
    }

    [RelayCommand]
    private async void AddAbility()
    {
        var ability = new Ability();
        Abilities.Add(new AbilityViewModel(ability));
        await _dataService.SaveAddAsync(ability);
        EditAbility(ability);
    }

    public AbilityCRUDViewModel(IDataService dataService, INavigationService navigationService, IFilteringService filteringService)
    {
        _dataService = dataService;
        _navigationService = navigationService;
        _filteringService = filteringService;

        //var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        //_filterTimer = dispatcherQueue.CreateTimer();

        WeakReferenceMessenger.Default.Register<AbilityCRUDRequestMessage>(this, (r, m) =>
        {
            HandleCRUDRequest(m.Parameter.Ability, m.RequestType);
        });

    }
    public void OnNavigatedFrom()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
        //_filterTimer.Stop();
    }
    public async void OnNavigatedTo(object parameter)
    {
        Abilities.Clear();
        foreach (var ability in await _dataService.GetAllSpellsAsync())
            Abilities.Add(new AbilityViewModel(ability));


        _abilityCache = new List<AbilityViewModel>(Abilities);
        SearchSuggestions = new(Abilities);
        MaximumSpellLevelFilter = SpellLevel.LevelNine;
        ConcentrationFilterSelected = ThreeStateBoolean.Either;
        MinimumSpellLeveLFilter = SpellLevel.Cantrip;
        DamageTypeFilterSelected = DamageType.Untyped;
        ResolutionTypeFilterSelected = ResolutionType.Undefined;
        SpellSchoolFilterSelected = MagicSchool.None;
        
    }

    private void HandleCRUDRequest(Ability ability, CRUDRequestType requestType)
    {
        if (requestType == CRUDRequestType.Delete)
        {
            DeleteAbility(ability);
        }
        else if (requestType == CRUDRequestType.Edit)
        {
            EditAbility(ability);
        }
        else if (requestType == CRUDRequestType.Copy)
        {
            CopyAbility(ability);
        }
    }

}
