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
//using EasyEncounters.Persistence.ApiToModel;
using EasyEncounters.Services.Filter;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml.Controls;

namespace EasyEncounters.ViewModels;

public partial class AbilityCRUDViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService _dataService;
    private readonly IFilteringService _filteringService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private AbilityFilter _abilityFilterValues;

    public AbilityCRUDViewModel(IDataService dataService, INavigationService navigationService, IFilteringService filteringService)
    {
        _dataService = dataService;
        _navigationService = navigationService;
        _filteringService = filteringService;
        _abilityFilterValues = (AbilityFilter)filteringService.GetFilterValues<Ability>();
    }


    public void OnNavigatedFrom()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public async void OnNavigatedTo(object parameter)
    {

        await AbilityFilterValues.ResetAsync();

    }

    [RelayCommand]
    private async Task AddAbility()
    {
        var ability = new Ability();
        //Abilities.Add(ability);
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
                await _dataService.SaveAddAsync(copied);//.Add(copied);
        }
    }

    [RelayCommand]
    private async Task DeleteAbility(object parameter)
    {
        if (parameter != null && parameter is Ability)
        {
            var toDelete = (Ability)parameter;
            //Abilities.Remove(Abilities.First(x => x == toDelete));
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

}