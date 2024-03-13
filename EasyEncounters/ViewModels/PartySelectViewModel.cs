using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;

namespace EasyEncounters.ViewModels;

public partial class PartySelectViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService _dataService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private string campaignName = "";

    public PartySelectViewModel(IDataService dataService, INavigationService navigationService)
    {
        _navigationService = navigationService;
        _dataService = dataService;
    }

    public ObservableCollection<Party> Parties { get; private set; } = new ObservableCollection<Party>();

    public void OnNavigatedFrom()
    {
    }

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is Campaign)
        {
            Parties.Clear();
            var campaign = parameter as Campaign;
            CampaignName = campaign?.Name ?? "";
            var parties = await _dataService.GetCampaignPartiesAsync(campaign);
            foreach (var party in parties)
                Parties.Add(party);
        }
    }

    [RelayCommand]
    private void PartySelected(Party party)
    {
        _navigationService.NavigateTo(typeof(EncounterSelectViewModel).FullName!, party);
    }
}