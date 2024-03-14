using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.UI;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Messages;
using Microsoft.UI.Dispatching;

namespace EasyEncounters.ViewModels;

public partial class PartyEditViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService _dataService;

    private readonly INavigationService _navigationService;

    private readonly DispatcherQueueTimer _filterTimer;

    [ObservableProperty]
    private Party? _party;

    [ObservableProperty]
    private Campaign? _selectedCampaign;

    public PartyEditViewModel(IDataService dataService, INavigationService navigationService)
    {
        _dataService = dataService;
        _navigationService = navigationService;

        var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        _filterTimer = dispatcherQueue.CreateTimer();

        //WeakReferenceMessenger.Default.Register<CreatureDeleteRequestMessage>(this, (r, m) =>
        //{
        //    RemoveCreature(m.Parameter.Creature);
        //});
    }

    public ObservableCollection<Campaign> Campaigns
    {
        get; private set;
    } = new();

    public ObservableCollection<CreatureViewModel> Creatures
    {
        get; private set;
    } = new();

    public ObservableCollection<CreatureViewModel> PartyMembers
    {
        get; private set;
    } = new();

    public void OnNavigatedFrom()
    {
        _filterTimer.Stop();
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
                PartyMembers.Add(new CreatureViewModel(partyMember));
            }

            Creatures.Clear();
            //todo: will have to  handle campaigns and creatures differently in future when there are a large number; can't possibly load all of each.
            foreach (var creature in await _dataService.GetAllCreaturesAsync())
            {
                Creatures.Add(new CreatureViewModel(creature));
            }

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
            PartyMembers.Add(new CreatureViewModel(creature));
            Party?.Members.Add(creature);
        }
        else if (parameter != null && parameter is CreatureViewModel creatureViewModel)
        {
            PartyMembers.Add(new CreatureViewModel(creatureViewModel.Creature));
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

    [RelayCommand]
    private async Task Filter(object parameter)
    {
        _filterTimer.Debounce(async () =>
        {
            await FilterAsync(parameter);
        }, TimeSpan.FromSeconds(0.3));
    }

    private async Task FilterAsync(object parameter)
    {
        if (parameter is string text)
        {

            //remove is worse performance than clearing and repopulating the list, but much less 'flickery'.

            var matched = Creatures.Where(x => x.Creature.Name.Contains(text, StringComparison.InvariantCultureIgnoreCase)).ToList();
            List<CreatureViewModel> noMatch = new();

            for (var i = Creatures.Count - 1; i >= 0; i--)
            {
                var item = Creatures[i];
                if (!matched.Contains(item))
                {
                    Creatures.Remove(item);
                    noMatch.Add(item);
                }
            }

            foreach (var item in noMatch)
            {
                Creatures.Add(item);
            }
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
        if (parameter != null && parameter is Creature creature)
        {
            PartyMembers.Remove(PartyMembers.First(x => x.Creature == creature));
            Party?.Members.Remove(creature);
        }
    }
}