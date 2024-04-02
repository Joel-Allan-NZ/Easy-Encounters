using System.Collections.ObjectModel;
using System.Formats.Asn1;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Services;
using EasyEncounters.Messages;
using EasyEncounters.Models;
using EasyEncounters.Services.Filter;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml.Controls;

namespace EasyEncounters.ViewModels;

public partial class EncounterAddCreaturesTabViewModel : ObservableRecipientTab
{
    private readonly IDataService _dataService;
    private readonly IFilteringService _filteringService;

    private ActiveEncounter? _activeEncounter;
    private IList<ObservableCreature>? _creatureCache;

    [ObservableProperty]
    private CreatureFilter _creatureFilterValues;

    [ObservableProperty]
    private double _maximumCRFilter;

    [ObservableProperty]
    private double _minimumCRFilter;

    [ObservableProperty]
    private List<ObservableCreature>? searchSuggestions;

    public EncounterAddCreaturesTabViewModel(IDataService dataService, IFilteringService filteringService)
    {
        _filteringService = filteringService;
        _dataService = dataService;
        _activeEncounter = null;

        _creatureCache = new List<ObservableCreature>();
        _creatureFilterValues = (CreatureFilter)_filteringService.GetFilterValues<Creature>();
    }

    //public event EventHandler<DataGridColumnEventArgs>? Sorting;

    public List<ObservableCreature> Creatures
    {
        get; private set;
    } = new();

    /// <summary>
    /// Additional creatures to add to the active encounter, and their quantities
    /// </summary>
    public ObservableCollection<ObservableKVP<ObservableCreature, int>> EncounterCreaturesByCount
    {
        get; private set;
    } = new();

    public override void OnTabClosed()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public async override void OnTabOpened(object? parameter)
    {
        if (parameter != null && parameter is ActiveEncounter)
        {
            _activeEncounter = (ActiveEncounter)parameter;
        }
        await CreatureFilterValues.ResetAsync();
    }

    [RelayCommand]
    private void AddCreature(object obj)
    {
        if (obj != null && obj is ObservableCreature creature)
        {


            var match = EncounterCreaturesByCount.FirstOrDefault(x => x.Key.Creature.Equals(creature.Creature));

            if (match == null)
            {
                EncounterCreaturesByCount.Add(new ObservableKVP<ObservableCreature, int>(creature, 1));
            }
            else
                match.Value++;
        }
    }

    [RelayCommand]
    private void CommitChanges(object obj)
    {

        WeakReferenceMessenger.Default.Send(new AddCreaturesRequestMessage(EncounterCreaturesByCount));

        //todo: send message to encounter tab, ensuring list of vms repopulated.
        //todo: close tab when finished.
        //NB: doesn't set their initiative currently, but you can drag-and-drop it in the active encounter
    }

    [RelayCommand]
    private void RemoveCreature(object obj)
    {
        if (obj != null && obj is ObservableCreature)
        {
            ObservableCreature toRemove = (ObservableCreature)obj;

            var match = EncounterCreaturesByCount.FirstOrDefault(x => x.Key.Creature.Equals(toRemove.Creature));
            if (match != null)
                EncounterCreaturesByCount.Remove(match);
        }
    }

}