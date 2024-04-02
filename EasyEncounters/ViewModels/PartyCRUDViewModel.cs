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
using EasyEncounters.Persistence.SQLLite;
using EasyEncounters.Services.Filter;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml.Controls;

namespace EasyEncounters.ViewModels;

public partial class PartyCRUDViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService _dataService;
    private readonly IFilteringService _filteringService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private PartyFilter _partyFilterValues;

    public PartyCRUDViewModel(IDataService dataService, INavigationService navigationService, IFilteringService filteringService)
    {
        _dataService = dataService;
        _navigationService = navigationService;
        _filteringService = filteringService;

        _partyFilterValues = (PartyFilter)_filteringService.GetFilterValues<Party>();
    }

    public void OnNavigatedFrom()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public async void OnNavigatedTo(object parameter)
    {
        await PartyFilterValues.ResetAsync();
    }

    [RelayCommand]
    private async Task AddParty()
    {
        var party = new Party();
        await _dataService.SaveAddAsync(party);
    }

    [RelayCommand]
    private async Task CopyParty(object parameter)
    {
        if (parameter != null && parameter is Party)
        {
            var copied = await _dataService.CopyAsync(parameter as Party);
            if (copied != null)
            {
                await _dataService.SaveAddAsync(copied);
                await PartyFilterValues.RefreshAsync();
            }
        }
    }

    [RelayCommand]
    private async Task DeleteParty(object parameter)
    {
        if (parameter != null && parameter is Party)
        {
            var toDelete = (Party)parameter;
            await _dataService.DeleteAsync(toDelete); 
            await PartyFilterValues.RefreshAsync();
        }
    }

    [RelayCommand]
    private void EditParty(object parameter)
    {
        if (parameter is not null and Party)
        {
            _navigationService.NavigateTo(typeof(PartyEditViewModel).FullName!, parameter);
        }
      
    }
}