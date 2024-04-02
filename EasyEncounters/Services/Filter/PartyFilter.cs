using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyEncounters.Services.Filter;

public partial class PartyFilter : GridFilteredValues<Party>
{
    private readonly string _safeTag = "PartyName";

    [ObservableProperty]
    private List<Party> _searchSuggestions = new();

    [ObservableProperty]
    private string _searchString;

    private readonly IDataService _dataService;

    [ObservableProperty]
    private List<Party> _data;

    public PartyFilter(IDataService dataService)
    {
        _sortTag = _safeTag;
        Data = new();
        _dataService = dataService;
        _sortAscending = true;
        SearchString = "";
        ResetAsync();

        _namesCache = (from a in _dataService.Parties() select a.Name).ToList();
    }
    public override IQueryable<Party> FilterAndSortQuery<U>(IQueryable<Party> queryable, U? additionalData, DataGridColumnEventArgs? e = null) where U : class
    {
        HandleDataGrid(e, _safeTag);

        queryable = Filter(queryable);

        return _sortTag switch
        {
            "PartyName" => _sortAscending ? queryable.OrderBy(x => x.Name) : queryable.OrderByDescending(x => x.Name),
            "PartyMemberCount" => _sortAscending ? queryable.OrderBy(x => x.Members.Count) : queryable.OrderByDescending(x => x.Members.Count),
            "PartyCampaign" => _sortAscending ? queryable.OrderBy(x => x.Campaign != null).ThenBy(x => x.Campaign.Name) : queryable.OrderByDescending(x => x.Campaign != null).ThenByDescending(x => x.Campaign.Name),
            _ => throw new ArgumentException($"{_sortTag} is not a valid sorting field on {typeof(Party).Name}")
        };

    }

    private IQueryable<Party> Filter(IQueryable<Party> queryable)
    {
        if (!string.IsNullOrEmpty(SearchString))
        {
            return queryable.Where(x => x.Name.ToLower().Contains(SearchString.ToLower()));
        }
        return queryable;
    }

    public async override Task ResetAsync()
    {
        SearchString = "";
        await GetPaginatedList(1, 50);
    }

    public async override Task GetPaginatedList(int pageIndex, int pageSize, DataGridColumnEventArgs? e = null)
    {
        var query = FilterAndSortQuery<Creature>(_dataService.Parties().Include(x => x.Members), null, e);

        var pagedEncounters = await PaginatedList<Party>.CreateAsync(
            query,
            (x) => (Party)x,
            pageIndex,
            pageSize);

        PageNumber = pagedEncounters.PageIndex;
        PageCount = pagedEncounters.PageCount;
        Data = pagedEncounters;
    }

}
