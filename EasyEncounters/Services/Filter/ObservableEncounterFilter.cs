using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Core.Models;
using CommunityToolkit.WinUI.UI.Controls;
using System.Collections.ObjectModel;
using EasyEncounters.Models;

namespace EasyEncounters.Services.Filter;
public partial class ObservableEncounterFilter : FilterValues
{
    private readonly IList<EncounterDifficulty> _difficulties = Enum.GetValues(typeof(EncounterDifficulty)).Cast<EncounterDifficulty>().ToList();

    [ObservableProperty]
    private EncounterDifficulty _maximumDifficulty;

    [ObservableProperty]
    private int _maximumEnemiesFilter;

    [ObservableProperty]
    private EncounterDifficulty _minimumDifficulty;

    [ObservableProperty]
    private int _minimumEnemiesFilter;

    [ObservableProperty]
    private List<ObservableEncounter> _searchSuggestions = new();

    [ObservableProperty]
    private string _campaignName;



    public ObservableEncounterFilter()
    {
        CampaignName = "";
        ResetFilter();
    }
    public ICollection<FilterCriteria<ObservableEncounter>> GenerateFilterCriteria(string text)
    {
        List<FilterCriteria<ObservableEncounter>> criteriaList = new()
        {
            new(x => x.EncounterDifficulty, MinimumDifficulty, MaximumDifficulty),
            new(x => x.Encounter.Creatures.Count, MinimumEnemiesFilter, MaximumEnemiesFilter),
        };

        if (!String.IsNullOrEmpty(text))
        {
            criteriaList.Add(new(x => x.Encounter.Name, text));
        }

        if (!String.IsNullOrEmpty(CampaignName))
        {
            criteriaList.Add(new(x => x.Encounter.IsCampaignOnlyEncounter, true, true));
            criteriaList.Add(new(x => x.Encounter.Campaign.Name, CampaignName)); //dereference risk here should be safe - IsCampaignOnlyEncounter evaluated first implies that only Encounters with a Campaign left.
        }

        return criteriaList;
    }

    public IList<EncounterDifficulty> Difficulties => _difficulties;

    public void ResetFilter()
    {
        CampaignName = "";
        MaximumEnemiesFilter = 50;
        MaximumDifficulty = EncounterDifficulty.VeryDeadly;
        MinimumEnemiesFilter = 0;
        MinimumDifficulty = EncounterDifficulty.None;
    }

    public void SortCollection(ObservableCollection<ObservableEncounter> collection, DataGridColumnEventArgs e)
    {
        var sortDirection = e.Column.SortDirection == DataGridSortDirection.Ascending;

        var tagString = e.Column.Tag.ToString();

        Func<ObservableEncounter, object> predicate = tagString switch
        {
            "EncounterName" => new(x => x.Encounter.Name),
            "EncounterDifficulty" => new(x => x.EncounterDifficulty),
            "EncounterEnemyCount" => new(x => x.Encounter.Creatures.Count),
            _ => throw new Exception("Not a valid tag name")
        };

        SortByPredicate(collection, predicate, sortDirection);


        if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
        {
            e.Column.SortDirection = DataGridSortDirection.Ascending;
        }
        else
        {
            e.Column.SortDirection = DataGridSortDirection.Descending;
        }
    }
}
