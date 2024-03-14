using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Models;

namespace EasyEncounters.ViewModels;

public partial class CampaignCRUDViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService _dataService;

    public CampaignCRUDViewModel(IDataService dataService)
    {
        _dataService = dataService;
    }

    public ObservableCollection<ObservableCampaign> Campaigns
    {
        get; private set;
    } = new();

    public void OnNavigatedFrom()
    {
        //todo: ensure changes have saved - slightly different in this single VM w/ datagrid. Best to just convert it to the same
        //scheme as all others for consistency, but too lazy rn
        //await _dataService.
    }

    public async void OnNavigatedTo(object parameter)
    {
        Campaigns.Clear();
        foreach (var campaign in await _dataService.GetAllCampaignsAsync())
        {
            ObservableCampaign observable = new(campaign);
            observable.PropertyChanged += OnCampaignPropertyChanged;
            Campaigns.Add(observable);
        }
    }

    private async void OnCampaignPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if(sender is not null and ObservableCampaign observable)
        {
            await _dataService.SaveAddAsync(observable.Campaign);
        }
    }

    [RelayCommand]
    private async Task AddNewCampaign()
    {
        var campaign = new Campaign();
        Campaigns.Add(new(campaign));
        await _dataService.SaveAddAsync(campaign);
    }
}