﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Services;
using EasyEncounters.Messages;
using EasyEncounters.Views;

namespace EasyEncounters.ViewModels;
public partial class PartyCRUDViewModel : ObservableRecipient, INavigationAware
{
    public ObservableCollection<PartyViewModel> Parties
    {
        get; private set;
    } = new();

    [RelayCommand]
    private async Task AddNewParty()
    {
        var party = new Party();
        Parties.Add(new PartyViewModel(party));
        await _dataService.SaveAddAsync(party);
    }

    [RelayCommand]
    private async Task DeleteParty(object parameter)
    {
        if (parameter != null && parameter is Party)
        {
            var toDelete = (Party)parameter;
            Parties.Remove(Parties.First(x => x.Party == toDelete));
            await _dataService.DeleteAsync(toDelete);
        }
    }

    [RelayCommand]
    private async Task CopyParty(object parameter)
    {
        if (parameter != null && parameter is Party)
        {
            var copied = await _dataService.CopyAsync(parameter as Party);
            if (copied != null)
                Parties.Add(new PartyViewModel(copied));
        }
    }

    [RelayCommand]
    private void EditParty(object parameter)
    {
        if (parameter != null && parameter is Party)
        {
            _navigationService.NavigateTo(typeof(PartyEditViewModel).FullName!, parameter);
        }
        else if(parameter != null && parameter is PartyViewModel)
        {
            _navigationService.NavigateTo(typeof(PartyEditViewModel).FullName!, ((PartyViewModel)parameter).Party);
        }
    }

    private readonly INavigationService _navigationService;
    private readonly IDataService _dataService;

    public PartyCRUDViewModel(IDataService dataService, INavigationService navigationService)
    {
        _dataService = dataService;
        _navigationService = navigationService;

        WeakReferenceMessenger.Default.Register<PartyCopyRequestMessage>(this, (r, m) =>
        {
            _ = CopyParty(m.Parameter.Party);
        });
        WeakReferenceMessenger.Default.Register<PartyDeleteRequestMessage>(this, (r, m) =>
        {
            _ = DeleteParty(m.Parameter.Party);
        });
        WeakReferenceMessenger.Default.Register<PartyEditRequestMessage>(this, (r, m) =>
        {
            EditParty(m.Parameter.Party);
        });


    }
    public void OnNavigatedFrom()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
    public async void OnNavigatedTo(object parameter)
    {
        Parties.Clear();
        foreach (var party in await _dataService.GetAllPartiesAsync())
            Parties.Add(new PartyViewModel(party));
    }
}
