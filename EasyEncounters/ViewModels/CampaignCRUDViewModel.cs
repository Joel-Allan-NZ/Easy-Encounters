using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
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
    private readonly int _pageSize = 50;

    public CampaignCRUDViewModel(IDataService dataService)
    {
        _dataService = dataService;
    }

    public List<ObservableCampaign> Campaigns
    {
        get; private set;
    } = new();

    [ObservableProperty]
    private int _pageCount;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(FirstAsyncCommand))]
    [NotifyCanExecuteChangedFor(nameof(PreviousAsyncCommand))]
    [NotifyCanExecuteChangedFor(nameof(NextAsyncCommand))]
    [NotifyCanExecuteChangedFor(nameof(LastAsyncCommand))]
    private int _pageNumber;

    public void OnNavigatedFrom()
    {
        //todo: ensure changes have saved - slightly different in this single VM w/ datagrid. Best to just convert it to the same
        //scheme as all others for consistency, but too lazy rn
        //await _dataService.
    }

    public async void OnNavigatedTo(object parameter)
    {
        Campaigns.Clear();
        await GetCampaigns(1, _pageSize);
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

    private bool CanFirstAsync() => PageNumber != 1;
    private bool CanPreviousAsync() => PageNumber > 1;
    private bool CanNextAsync() => PageNumber < PageCount;
    private bool CanLastAsync() => PageCount != PageCount;

    private async Task GetCampaigns(int pageIndex, int pageSize)
    {
        var pagedCampaigns = await PaginatedList<ObservableCampaign>.CreateAsync(
            _dataService.Campaigns(),
            (x) => new ObservableCampaign((Campaign)x),
            pageIndex,
            pageSize);
        PageNumber = pagedCampaigns.PageIndex;
        PageCount = pagedCampaigns.PageCount;
        Campaigns = pagedCampaigns;

    }

    [RelayCommand(CanExecute = nameof(CanFirstAsync))]
    private async void FirstAsync() => await GetCampaigns(1, _pageSize);

    [RelayCommand(CanExecute = nameof(CanPreviousAsync))]
    private async void PreviousAsync() => await GetCampaigns(PageNumber-1, _pageSize);

    [RelayCommand(CanExecute = nameof(CanNextAsync))]
    private async void NextAsync() => await GetCampaigns(PageNumber + 1, _pageSize);

    [RelayCommand(CanExecute = nameof(CanLastAsync))]
    private async void LastAsync() => await GetCampaigns(PageCount, _pageSize);

}