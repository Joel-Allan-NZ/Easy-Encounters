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
    private IList<ObservableCreature> _creatureCache;

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

        _creatureCache = new List<ObservableCreature>();
        _creatureFilterValues = (CreatureFilter)_filteringService.GetFilterValues<ObservableCreature>();
        //WeakReferenceMessenger.Default.Register<CreatureDeleteRequestMessage>(this, (r, m) =>
        //{
        //    RemoveCreature(m.Parameter.Creature);
        //});
    }

    public ObservableCollection<Campaign> Campaigns
    {
        get; private set;
    } = new();

    public ObservableCollection<ObservableCreature> Creatures
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

            Creatures.Clear();
            //todo: will have to  handle campaigns and creatures differently in future when there are a large number; can't possibly load all of each.
            foreach (var creature in await _dataService.GetAllCreaturesAsync())
            {
                Creatures.Add(new ObservableCreature((Creature)creature));
            }
            _creatureCache = new List<ObservableCreature>(Creatures);

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

    [RelayCommand]
    private void ClearCreatureFilter()
    {
        CreatureFilterValues.ResetFilter();
        CreatureFilter("");
    }

    [RelayCommand]
    private void CreatureFilter(string text)
    {
        var filtered = _filteringService.Filter(_creatureCache, CreatureFilterValues, text);
        Creatures.Clear();
        foreach (var creature in filtered)
            Creatures.Add(creature);
    }

    [RelayCommand]
    private void SearchTextChange(string text)
    {
        var filtered = _filteringService.Filter(_creatureCache, CreatureFilterValues, text);
        if (String.IsNullOrEmpty(text))
        {
            Creatures.Clear();
            foreach (var creature in filtered)
                Creatures.Add(creature);
        }
    }

    [RelayCommand]
    private void DataGridSort(DataGridColumnEventArgs e)
    {
        CreatureFilterValues.SortCollection(Creatures, e);
    }
}