﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;

namespace EasyEncounters.ViewModels;
public partial class CampaignCRUDViewModel : ObservableRecipient, INavigationAware
{
    public ObservableCollection<Campaign> Campaigns
    {
        get; private set;
    } = new();

    [RelayCommand]
    private async Task AddNewCampaign()
    {
        var campaign = new Campaign();
        await _dataService.SaveAddAsync(campaign);
    }


    private readonly IDataService _dataService;

    public CampaignCRUDViewModel(IDataService dataService)
    {
        _dataService = dataService;
    }

    public void OnNavigatedFrom()
    {
        //todo: ensure changes have saved - slightly different in this single VM w/ datagrid. Best to just convert it to the same
        //scheme as all others for consistency, but too lazy rn
        //await _dataService.
    }
    public async void OnNavigatedTo(object parameter)
    {
        Campaigns.Clear();
        foreach(var campaign in await _dataService.GetAllCampaignsAsync())
        {
            Campaigns.Add(campaign);
        }
    }
}
