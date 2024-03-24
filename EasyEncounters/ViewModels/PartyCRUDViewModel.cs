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
using EasyEncounters.Persistence.SQLLite;
using EasyEncounters.Services.Filter;

namespace EasyEncounters.ViewModels;

public partial class PartyCRUDViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService _dataService;
    private readonly IFilteringService _filteringService;
    private readonly INavigationService _navigationService;
    private IList<Party>? _partyCache;

    [ObservableProperty]
    private PartyFilter _partyFilterValues;

    public PartyCRUDViewModel(IDataService dataService, INavigationService navigationService, IFilteringService filteringService, CopyDB db)
    {
        _dataService = dataService;
        _navigationService = navigationService;
        _filteringService = filteringService;

        _partyFilterValues = (PartyFilter)_filteringService.GetFilterValues<Party>();

        //db.CopyAll();
        
        //db.CopyCampaigns();
        //db.CopyParties();
        //db.CopyCreatures();
        //db.CopyEncounters();
        //db.CopyAbilities();
        

        //WeakReferenceMessenger.Default.Register<PartyCopyRequestMessage>(this, (r, m) =>
        //{
        //    _ = CopyParty(m.Parameter.Party);
        //});
        //WeakReferenceMessenger.Default.Register<PartyDeleteRequestMessage>(this, (r, m) =>
        //{
        //    _ = DeleteParty(m.Parameter.Party);
        //});
        //WeakReferenceMessenger.Default.Register<PartyEditRequestMessage>(this, (r, m) =>
        //{
        //    EditParty(m.Parameter.Party);
        //});
    }

    public ObservableCollection<Party> Parties
    {
        get; private set;
    } = new();

    public void OnNavigatedFrom()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public async void OnNavigatedTo(object parameter)
    {
        Parties.Clear();
        foreach (var party in await _dataService.GetAllPartiesAsync())
        {
            Parties.Add(party);
        }

        _partyCache = new List<Party>(Parties);
    }

    [RelayCommand]
    private async Task AddNewParty()
    {
        var party = new Party();
        Parties.Add(party);
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

                Parties.Add(copied);
                _partyCache?.Add(copied);
            }
        }
    }

    [RelayCommand]
    private async Task DeleteParty(object parameter)
    {
        if (parameter != null && parameter is Party)
        {
            var toDelete = (Party)parameter;
            Parties.Remove(toDelete);
            await _dataService.DeleteAsync(toDelete);
        }
    }

    [RelayCommand]
    private void EditParty(object parameter)
    {
        if (parameter is not null and Party)
        {
            _navigationService.NavigateTo(typeof(PartyEditViewModel).FullName!, parameter);
        }
        //else if (parameter != null && parameter is PartyViewModel)
        //{
        //    _navigationService.NavigateTo(typeof(PartyEditViewModel).FullName!, ((PartyViewModel)parameter).Party);
        //}
    }

    [RelayCommand]
    private void PartyFilter(string text)
    {
        if (_partyCache == null)
            return;

        var filtered = _filteringService.Filter(_partyCache, PartyFilterValues, text);
        Parties.Clear();
        foreach (var party in filtered)
        {
            Parties.Add(party);
        }    
    }

    [RelayCommand]
    private void DataGridSort(DataGridColumnEventArgs e)
    {
        PartyFilterValues.SortCollection(Parties, e);
    }

    [RelayCommand]
    private void SearchTextChange(string text)
    {
        if (_partyCache == null)
            return;

        var filtered = _filteringService.Filter(_partyCache, PartyFilterValues, text);
        if (String.IsNullOrEmpty(text))
        {
            Parties.Clear();
            foreach (var party in filtered)
            {
                Parties.Add(party);
            }
        }
    }
}