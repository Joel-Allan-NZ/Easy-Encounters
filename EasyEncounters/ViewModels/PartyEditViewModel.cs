using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.UI;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Messages;
using EasyEncounters.Services.Filter;
using Microsoft.UI.Dispatching;

namespace EasyEncounters.ViewModels;

public partial class PartyEditViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService _dataService;
    private readonly IFilteringService _filteringService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private Party? _party;

    [ObservableProperty]
    private Campaign? _selectedCampaign;


    [ObservableProperty]
    private CreatureFilter _creatureFilterValues;

    public PartyEditViewModel(IDataService dataService, INavigationService navigationService, IFilteringService filteringService)
    {
        _dataService = dataService;
        _navigationService = navigationService;
        _filteringService = filteringService;
        _creatureFilterValues = (CreatureFilter)_filteringService.GetFilterValues<Creature>();
    }

    public ObservableCollection<Campaign> Campaigns
    {
        get; private set;
    } = new();

    public ObservableCollection<ObservableCreature> PartyMembers
    {
        get; private set;
    } = new();

    public void OnNavigatedFrom()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter != null && parameter is Party party)
        {
            Party = party;

            PartyMembers.Clear();
            foreach (var partyMember in Party.Members)
            {
                PartyMembers.Add(new ObservableCreature(partyMember));
            }

            await CreatureFilterValues.ResetAsync();

            Campaigns.Clear();
            foreach (var campaign in await _dataService.GetAllCampaignsAsync())
            {
                Campaigns.Add(campaign);
            }

            SelectedCampaign = Party.Campaign ?? Campaigns.First();
        }
    }

    [RelayCommand]
    private void AddCreature(object parameter)
    {
        if (parameter != null && parameter is Creature creature)
        {
            PartyMembers.Add(new ObservableCreature(creature));
            Party?.Members.Add(creature);
        }
        else if (parameter != null && parameter is ObservableCreature creatureViewModel)
        {
            PartyMembers.Add(new ObservableCreature(creatureViewModel.Creature));
            Party?.Members.Add(creatureViewModel.Creature);
        }
    }

    [RelayCommand]
    private async Task CommitChanges(object obj)
    {
        await _dataService.SaveAddAsync(Party);
        if (_navigationService.CanGoBack)
        {
            _navigationService.GoBack();
        }
    }

    partial void OnSelectedCampaignChanged(Campaign? value)
    {
        if (Party?.Campaign != value && Party != null)
            Party.Campaign = value;
    }

    [RelayCommand]
    private void RemoveCreature(object parameter)
    {
        if (parameter != null && parameter is ObservableCreature observableCreature)
        {
            var creature = observableCreature.Creature;
            PartyMembers.Remove(PartyMembers.First(x => x.Creature == creature));
            Party?.Members.Remove(creature);
        }
    }
}