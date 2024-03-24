using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Messages;
using EasyEncounters.Models;
using EasyEncounters.Persistence.ApiToModel;
using EasyEncounters.Services.Filter;

namespace EasyEncounters.ViewModels;

public partial class AbilityCRUDViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService _dataService;
    private readonly IFilteringService _filteringService;
    private readonly INavigationService _navigationService;
    private List<Ability>? _abilityCache;

    [ObservableProperty]
    private AbilityFilter _abilityFilterValues;

    public AbilityCRUDViewModel(IDataService dataService, INavigationService navigationService, IFilteringService filteringService)
    {
        _dataService = dataService;
        _navigationService = navigationService;
        _filteringService = filteringService;
        _abilityFilterValues = (AbilityFilter)filteringService.GetFilterValues<Ability>();
    }

    public ObservableCollection<Ability> Abilities
    {
        get; set;
    } = new();

    public void OnNavigatedFrom()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
        //_filterTimer.Stop();
    }

    public async void OnNavigatedTo(object parameter)
    {
        Abilities.Clear();
        foreach (var ability in await _dataService.GetAllSpellsAsync())
            Abilities.Add(ability);

        AbilityFilterValues = (AbilityFilter)_filteringService.GetFilterValues<Ability>();

        _abilityCache = new List<Ability>(Abilities);

        //foreach (var ability in await ReadDNDJson.ReadAbilities())
        //{
        //    var match = Abilities.FirstOrDefault(x => x.Name == ability.Name);
        //    if (match != null)
        //    {
        //        match.TimeDuration = ability.TimeDuration;
        //        match.EffectDescription = ability.EffectDescription;

        //        await _dataService.SaveAddAsync(match);
        //    }
        //}
        

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
    private async Task AddAbility()
    {
        var ability = new Ability();
        Abilities.Add(ability);
        await _dataService.SaveAddAsync(ability);
        EditAbility(ability);
    }

    [RelayCommand]
    private async Task CopyAbility(object ability)
    {
        if (ability != null && ability is Ability)
        {
            var copied = await _dataService.CopyAsync(ability as Ability);
            if (copied != null)
                Abilities.Add(copied);
        }
    }

    [RelayCommand]
    private void DataGridSort(DataGridColumnEventArgs e)
    {
        AbilityFilterValues.SortCollection(Abilities, e);
    }

    [RelayCommand]
    private async Task DeleteAbility(object parameter)
    {
        if (parameter != null && parameter is Ability)
        {
            var toDelete = (Ability)parameter;
            Abilities.Remove(Abilities.First(x => x == toDelete));
            await _dataService.DeleteAsync(toDelete);
        }
    }

    [RelayCommand]
    private void EditAbility(object ability)
    {
        if (ability != null && ability is Ability)
        {
            _navigationService.NavigateTo(typeof(AbilityEditViewModel).FullName!, ability);
        }
    }

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
    private void ClearFilters()
    {
        AbilityFilterValues.ResetFilter();
        AbilityFilter("");
    }
}