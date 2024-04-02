using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABI.System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyEncounters.Services.Filter;

public partial class EncounterFilter : GridFilteredValues<Encounter>
{
    private readonly IList<EncounterDifficulty> _difficulties = Enum.GetValues(typeof(EncounterDifficulty)).Cast<EncounterDifficulty>().ToList();
    private readonly int _maxCreatures = 255;
    private readonly IDataService _dataService;

    [ObservableProperty]
    private EncounterDifficulty _maximumDifficulty;

    [ObservableProperty]
    private int _maximumEnemiesFilter;

    [ObservableProperty]
    private EncounterDifficulty _minimumDifficulty;

    [ObservableProperty]
    private int _minimumEnemiesFilter;

    [ObservableProperty]
    private List<string> _searchSuggestions = new();

    [ObservableProperty]
    private string _campaignName;

    public IList<EncounterDifficulty> Difficulties => _difficulties;

    private readonly string _safeTag = "EncounterName";

    [ObservableProperty]
    private List<ObservableEncounter> _data;


    public EncounterFilter(IDataService dataService)
    {
        Data = new();
        _dataService = dataService;
        _sortAscending = true;
        _sortTag = _safeTag;
        SearchString = "";
        CampaignName = "";

        _namesCache = (from a in _dataService.Encounters()
                                select a.Name).ToList();
    }

    public override IQueryable<Encounter> FilterAndSortQuery<U>(IQueryable<Encounter> queryable, U? additionalData, DataGridColumnEventArgs? e = null) where U : class
    {
        HandleDataGrid(e, _safeTag);

        queryable = Filter(queryable);

        return _sortTag switch
        {
            "EncounterName" => _sortAscending ? queryable.OrderBy(x => x.Name) : queryable.OrderByDescending(x => x.Name),
            "EncounterDifficulty" => _sortAscending ? queryable.OrderBy(x => x.AdjustedEncounterXP) : queryable.OrderByDescending(x => x.AdjustedEncounterXP),
            "EncounterEnemyCount" => _sortAscending ? queryable.OrderBy(x => x.CreatureCount) : queryable.OrderByDescending(x => x.CreatureCount),
            "EncounterCampaign" => _sortAscending ? queryable.OrderBy(x => x.IsCampaignOnlyEncounter).ThenBy(x => x.Campaign.Name) : queryable.OrderByDescending(x => x.IsCampaignOnlyEncounter).ThenBy(x => x.Campaign.Name),
            _ => throw new ArgumentException($"{_sortTag} is not a valid sorting field on {typeof(Encounter).Name}")
        };

    }

    private IQueryable<Encounter> Filter(IQueryable<Encounter> queryable)
    {
        var result = queryable.Where(x => x.CreatureCount >= MinimumEnemiesFilter && x.CreatureCount <= MaximumEnemiesFilter);

        if (!string.IsNullOrEmpty(CampaignName))
        {
            result = result.Where(x => !x.IsCampaignOnlyEncounter || x.Campaign.Name.ToLower().Contains(CampaignName.ToLower()));
        }
        if (!string.IsNullOrEmpty(SearchString))
        {
            result = result.Where(x => x.Name.ToLower().Contains(SearchString.ToLower()));
        }

        return result;
    }


    public async override Task ResetAsync()
    {
        SearchString = "";
        CampaignName = "";
        MaximumEnemiesFilter = _maxCreatures;
        MaximumDifficulty = EncounterDifficulty.VeryDeadly;
        MinimumEnemiesFilter = 0;
        MinimumDifficulty = EncounterDifficulty.None;
        await GetPaginatedList(1, 50);
    }

    public async override Task GetPaginatedList(int pageIndex, int pageSize, DataGridColumnEventArgs? e = null)
    {
        var query = FilterAndSortQuery<Party>(_dataService.Encounters(), null, e);

        var pagedEncounters = await PaginatedList<ObservableEncounter>.CreateAsync(
            query,
            (x) => new ObservableEncounter((Encounter)x),
            pageIndex,
            pageSize);

        PageNumber = pagedEncounters.PageIndex;
        PageCount = pagedEncounters.PageCount;
        Data = pagedEncounters;
    }
}
