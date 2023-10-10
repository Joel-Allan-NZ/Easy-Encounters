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

    public IList<SpellLevel> SpellLevels => _spellLevels;

    private List<AbilityViewModel> _abilityCache;

    [ObservableProperty]
    private List<AbilityViewModel> _searchSuggestions;

    [ObservableProperty]
    private SpellLevel _minimumSpellLeveLFilter;

    [ObservableProperty]
    private SpellLevel _maximumSpellLevelFilter;


    public event EventHandler<DataGridColumnEventArgs> Sorting;
    protected virtual void OnSorting(DataGridColumnEventArgs e)
    {
        EventHandler<DataGridColumnEventArgs> handler = Sorting;
        if (handler != null)
            handler(this, e);
    }

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
            new FilterCriteria<AbilityViewModel>(x => x.Ability.Name, text)
        };

        var filtered = _filteringService.Filter(_abilityCache, criteria);
        Abilities.Clear();
        foreach (var ability in filtered)
            Abilities.Add(ability);
    }

    [RelayCommand]
    private void DataGridSort(DataGridColumnEventArgs e)
    {
        OnSorting(e);
        if (e.Column.Tag.ToString() == "AbilityName")
        {
            SortByPredicate(Abilities, x => x.Ability.Name, e.Column.SortDirection == DataGridSortDirection.Ascending);
            //if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
            //    SortAbilitiesByName(false);
            //else
            //    SortAbilitiesByName(true);
        }
        else if (e.Column.Tag.ToString() == "AbilitySpellLevel")
        {
                SortByPredicate(Abilities, x => x.Ability.SpellLevel, e.Column.SortDirection == DataGridSortDirection.Ascending);
        }
        else if(e.Column.Tag.ToString() == "AbilityDamageType")
        {
            SortByPredicate(Abilities, x => x.Ability.DamageTypes, e.Column.SortDirection == DataGridSortDirection.Ascending);
            //if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
            //    SortAbilitiesByDamageType(false);
            //else
            //    SortAbilitiesByDamageType(true);
        }
        else if(e.Column.Tag.ToString() == "AbilityResolutionType")
        {
            SortByPredicate(Abilities, x => x.Ability.Resolution, e.Column.SortDirection == DataGridSortDirection.Ascending);
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

    //private void SortAbilitiesbyLevel(bool ascending)
    //{
    //    if (!ascending)
    //        tmp = Creatures.OrderBy(x => x.Creature.Name).ToList();
    //    else
    //        tmp = Creatures.OrderByDescending(x => x.Creature.Name).ToList();

    //    Creatures.Clear();
    //    foreach (var creature in tmp)
    //        Creatures.Add(creature);
    //}

    //private void SortAbilitiesByName(bool ascending)
    //{
    
    //}

    //private void SortAbilitiesByDamageType(bool ascending)
    //{
    
    //}


    // DispatcherQueueTimer _filterTimer;


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

    //[RelayCommand]
    //private async void Filter(object parameter)
    //{
    //    _filterTimer.Debounce(async () =>
    //    {

    //        await FilterAsync(parameter);

    //    }, TimeSpan.FromSeconds(0.3));
    //}



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

    //private async Task FilterAsync(object parameter)
    //{
    //    if (parameter is string)
    //    {
    //        var text = (string)parameter;

    //        //remove is worse performance than clearing and repopulating the list, but much less 'flickery'.

    //        List<AbilityViewModel> matched = Abilities.Where(x => x.Ability.Name.Contains(text, StringComparison.InvariantCultureIgnoreCase)).ToList();
    //        List<AbilityViewModel> noMatch = new();


    //        for (var i = Abilities.Count - 1; i >= 0; i--)
    //        {
    //            var item = Abilities[i];
    //            if (!matched.Contains(item))
    //            {
    //                Abilities.Remove(item);
    //                noMatch.Add(item);
    //            }
    //        }

    //        foreach (var item in noMatch)
    //            Abilities.Add(item);

    //    }
    //}
}
