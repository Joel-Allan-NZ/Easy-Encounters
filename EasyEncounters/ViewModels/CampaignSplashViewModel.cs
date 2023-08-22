using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;

namespace EasyEncounters.ViewModels;
public partial class CampaignSplashViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService _dataService;
    private readonly INavigationService _navigationService;

    public ObservableCollection<Campaign> Campaigns { get; private set; } = new ObservableCollection<Campaign>();

    public CampaignSplashViewModel(IDataService dataService, INavigationService navigationService)
    {
        _dataService = dataService;
        _navigationService = navigationService;
    }

    public void OnNavigatedFrom()
    {
    
    }
    public async void OnNavigatedTo(object parameter)
    {
        //if (await _dataService.ActiveEncounterExistsAsync())
        //{
        //    _navigationService.NavigateTo(typeof(RunEncounterViewModel).FullName!, parameter);
        //}
        //else
        //{


            Campaigns.Clear();

            var data = await _dataService.GetAllCampaignsAsync();
            foreach (var item in data)
                Campaigns.Add(item);
        //}
    }

    [RelayCommand]
    private void CampaignSelected(object o)//(Campaign campaign)
    {
        if(o is Campaign)
            _navigationService.NavigateTo(typeof(PartySelectViewModel).FullName!, o as Campaign);
    }
}
