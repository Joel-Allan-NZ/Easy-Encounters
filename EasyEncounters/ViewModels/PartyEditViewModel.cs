using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Windows.ApplicationModel.Contacts;

namespace EasyEncounters.ViewModels;
public partial class PartyEditViewModel : ObservableRecipient, INavigationAware
{
    [ObservableProperty]
    private Party? _party;

    [ObservableProperty]
    private Campaign? _selectedCampaign;

    public ObservableCollection<CreatureViewModel> PartyMembers
    {
        get; private set;
    } = new();

    partial void OnSelectedCampaignChanged(Campaign? value)
    {
        if(Party?.Campaign != value && Party != null)
            Party.Campaign = value;
    }

    public ObservableCollection<CreatureViewModel> Creatures
    {
        get; private set;
    } = new();

    public ObservableCollection<Campaign> Campaigns
    {
        get; private set;
    } = new();

    [RelayCommand]
    private void AddCreature(object parameter)
    {
        if (parameter != null && parameter is Creature)
        {
            var creature = (Creature)parameter;
            PartyMembers.Add(new CreatureViewModel(creature));
            Party?.Members.Add(creature);

        }
        else if (parameter != null && parameter is CreatureViewModel)
        {
            var creatureViewModel = (CreatureViewModel)parameter;

            PartyMembers.Add(new CreatureViewModel(creatureViewModel.Creature));
            Party?.Members.Add(creatureViewModel.Creature);
        }
    }

    [RelayCommand]
    private void RemoveCreature(object parameter)
    {
        if (parameter != null && parameter is Creature)
        {
            var toRemove = (Creature)parameter;

            PartyMembers.Remove(PartyMembers.First(x => x.Creature == toRemove));
            Party?.Members.Remove(toRemove);
        }
    }


    [RelayCommand]
    private async Task CommitChanges(object obj)
    {
        await _dataService.SaveAddAsync(Party);
        if (_navigationService.CanGoBack)
            _navigationService.GoBack();
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
        if (parameter is string)
        {
            var text = (string)parameter;

            //remove is worse performance than clearing and repopulating the list, but much less 'flickery'.

            List<CreatureViewModel> matched = Creatures.Where(x => x.Creature.Name.Contains(text, StringComparison.InvariantCultureIgnoreCase)).ToList();
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
                Creatures.Add(item);

        }
    }

    private readonly IDataService _dataService;
    private readonly INavigationService _navigationService;
    private DispatcherQueueTimer _filterTimer;



    public PartyEditViewModel(IDataService dataService, INavigationService navigationService)
    {
        _dataService = dataService;
        _navigationService = navigationService;

        var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        _filterTimer = dispatcherQueue.CreateTimer();

        WeakReferenceMessenger.Default.Register<CreatureDeleteRequestMessage>(this, (r, m) =>
        {
            RemoveCreature(m.Parameter.Creature);
        });
    }
    public void OnNavigatedFrom()
    {
        _filterTimer.Stop();
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public async void OnNavigatedTo(object parameter)
    {
        if(parameter != null && parameter is Party)
        {
            Party = (Party)parameter;
            
            PartyMembers.Clear();
            foreach(var partyMember in Party.Members)
            {
                PartyMembers.Add(new CreatureViewModel(partyMember));
            }

            Creatures.Clear();
            //todo: will have to  handle campaigns and creatures differently in future when there are a large number; can't possibly load all of each.
            foreach (var creature in await _dataService.GetAllCreaturesAsync())
                Creatures.Add(new CreatureViewModel(creature));

            Campaigns.Clear();
            foreach (var campaign in await _dataService.GetAllCampaignsAsync()) 
                Campaigns.Add(campaign);

            SelectedCampaign = Party.Campaign ?? Campaigns.First();
        }
    }
}
