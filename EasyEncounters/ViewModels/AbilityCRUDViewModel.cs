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
using EasyEncounters.Services.Filter;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Windows.Media.Playback;

namespace EasyEncounters.ViewModels;
public partial class AbilityCRUDViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService _dataService;
    private readonly INavigationService _navigationService;
    private readonly IFilteringService _filteringService;

    [ObservableProperty]
    private AbilityFilter _abilityFilterValues;

    private List<AbilityViewModel>? _abilityCache;

    [RelayCommand]
    private void SearchTextChange(string text)
    {
        if (_abilityCache == null)
            return;

        var filtered = _filteringService.Filter(_abilityCache, AbilityFilterValues, text);
        if (String.IsNullOrEmpty(text))
        {
            Abilities.Clear();
            foreach (var ability in filtered)
                Abilities.Add(ability);
        }
    }

    [RelayCommand]
    private void AbilityFilter(string text)
    {
        if (_abilityCache == null)
            return;

        var filtered = _filteringService.Filter(_abilityCache, AbilityFilterValues, text);
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
    private async Task DeleteAbility(object parameter)
    {
        if (parameter != null && parameter is Ability)
        {
            var toDelete = (Ability)parameter;
            Abilities.Remove(Abilities.First(x => x.Ability == toDelete));
            await _dataService.DeleteAsync(toDelete);
        }
    }

    [RelayCommand]
    private void EditAbility(object ability)
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
    private async Task CopyAbility(object ability)
    {
        if(ability != null && ability is Ability)
        {
            var copied = await _dataService.CopyAsync(ability as Ability);
            if(copied != null)
                Abilities.Add(new AbilityViewModel(copied));
        }
    }

    [RelayCommand]
    private async Task AddAbility()
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
        _abilityFilterValues = (AbilityFilter)filteringService.GetFilterValues<AbilityViewModel>();

        WeakReferenceMessenger.Default.Register<AbilityCRUDRequestMessage>(this, (r, m) =>
        {
            _ = HandleCRUDRequest(m.Parameter.Ability, m.RequestType);
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

        AbilityFilterValues = (AbilityFilter)_filteringService.GetFilterValues<AbilityViewModel>();

        _abilityCache = new List<AbilityViewModel>(Abilities);
    }

    private async Task HandleCRUDRequest(Ability ability, CRUDRequestType requestType)
    {
        if (requestType == CRUDRequestType.Delete)
        {
            await DeleteAbility(ability);
        }
        else if (requestType == CRUDRequestType.Edit)
        {
            EditAbility(ability);
        }
        else if (requestType == CRUDRequestType.Copy)
        {
            await CopyAbility(ability);
        }
    }

}
