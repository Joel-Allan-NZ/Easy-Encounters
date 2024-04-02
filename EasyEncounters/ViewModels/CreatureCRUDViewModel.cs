using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Messages;
using EasyEncounters.Models;
//using EasyEncounters.Persistence.ApiToModel;
using EasyEncounters.Services.Filter;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml.Controls;
using Windows.Media.Audio;

namespace EasyEncounters.ViewModels;

public partial class CreatureCRUDViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService _dataService;
    private readonly IFilteringService _filteringService;
    private readonly INavigationService _navigationService;
    private readonly ICreatureService _creatureService;

    [ObservableProperty]
    private CreatureFilter _creatureFilterValues;

    public CreatureCRUDViewModel(IDataService dataService, INavigationService navigationService, IFilteringService filteringService, ICreatureService creatureService)
    {
        _dataService = dataService;
        _navigationService = navigationService;
        _filteringService = filteringService;
        _creatureService = creatureService;
        _creatureFilterValues = (CreatureFilter)_filteringService.GetFilterValues<Creature>();
    }


    public void OnNavigatedFrom()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public async void OnNavigatedTo(object parameter)
    {
        await CreatureFilterValues.ResetAsync();
    }

    [RelayCommand]
    private async Task AddNewCreature()
    {
        var creature = _creatureService.Create();

        await _dataService.SaveAddAsync(creature);
        _navigationService.NavigateTo(typeof(CreatureEditNavigationPageViewModel).FullName!, creature);
    }

    [RelayCommand]
    private async Task CopyCreature(object parameter)
    {
        if (parameter != null && parameter is Creature)
        {
            await _dataService.CopyAsync(parameter as Creature);
            await CreatureFilterValues.RefreshAsync();
        }
    }


    [RelayCommand]
    private async Task DeleteCreature(object parameter)
    {
       if (parameter != null && parameter is ObservableCreature creature)
       {
            var match = await _dataService.Creatures().FirstOrDefaultAsync(x => x.Id == creature.Creature.Id);
            if (match != null)
            {
                await _dataService.DeleteAsync(match);
                await CreatureFilterValues.RefreshAsync();
            }
       }
    }

    [RelayCommand]
    private void EditCreature(object parameter)
    {
        if (parameter is ObservableCreature)
        {
            //todo: pass a copy of the creature rather than the original, so changes are discarded if user hits back button rather than committing changes.
            //_navigationService.NavigateTo(typeof(CreatureEditViewModel).FullName!, ((CreatureViewModel)parameter).Creature);

            //experiemental:
            _navigationService.NavigateTo(typeof(CreatureEditNavigationPageViewModel).FullName!, ((ObservableCreature)parameter).Creature);
        }
    }
}