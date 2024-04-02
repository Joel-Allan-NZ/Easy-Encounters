using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Models;
using EasyEncounters.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EasyEncounters.Services.Filter;

public partial class CreatureFilter : GridFilteredValues<Creature>
{
    private readonly IList<CreatureType> _creatureTypes = Enum.GetValues<CreatureType>().Cast<CreatureType>().ToList();
    private readonly IList<CreatureSizeClass> _creatureSizeClasses = Enum.GetValues<CreatureSizeClass>().Cast<CreatureSizeClass>().ToList();
    private readonly IList<CreatureAlignment> _alignments = Enum.GetValues<CreatureAlignment>().Cast<CreatureAlignment>().ToList();
    private readonly string _safeTag = "CreatureName";
    private readonly int _maxCR = 30;
    private readonly int _minCR = 0;
    private readonly IDataService _dataService;

    [ObservableProperty]
    private List<ObservableCreature> _data;

    [ObservableProperty]
    private double _maximumCRFilter;

    [ObservableProperty]
    private double _minimumCRFilter;

    [ObservableProperty]
    private List<string> _searchSuggestions = new();

    [ObservableProperty]
    private CreatureType _creatureTypeFilterSelected;

    [ObservableProperty]
    private CreatureSizeClass _minCreatureSizeClassFilter;

    [ObservableProperty]
    private CreatureSizeClass _maxCreatureSizeClassFilter;

    [ObservableProperty]
    private CreatureAlignment _creatureAlignmentFilterSelected;

    //[ObservableProperty]
    //private string _searchString;

    public IList<CreatureAlignment> Alignments => _alignments;
    public IList<CreatureSizeClass> SizeClasses => _creatureSizeClasses;
    public IList<CreatureType> CreatureTypes => _creatureTypes;

    public CreatureFilter(IDataService dataService)
    {
        _dataService = dataService;
        _sortAscending = true;
        _sortTag = _safeTag;
        SearchString = "";
        Data = new();

        _namesCache = (from a in _dataService.Creatures() select a.Name).ToList();
    }

    public override IQueryable<Creature> FilterAndSortQuery<U>(IQueryable<Creature> queryable, U? additionalData, DataGridColumnEventArgs? e = null) where U : class
    {
        HandleDataGrid(e, _safeTag);

        queryable = Filter(queryable);

        return _sortTag switch
        {
            "CreatureName" => _sortAscending ? queryable.OrderBy(x => x.Name) : queryable.OrderByDescending(x => x.Name),
            "CreatureCR" => _sortAscending ? queryable.OrderBy(x => x.LevelOrCR) : queryable.OrderByDescending(x => x.LevelOrCR),
            "CreatureAlignment" => _sortAscending ? queryable.OrderBy(x => x.Alignment) : queryable.OrderByDescending(x => x.Alignment),
            "CreatureSize" => _sortAscending ? queryable.OrderBy(x => x.Size) : queryable.OrderByDescending(x => x.Size),
            "CreatureType" => _sortAscending ? queryable.OrderBy(x => x.CreatureType) : queryable.OrderByDescending(x => x.CreatureType),
            _ => throw new ArgumentException($"{_sortTag} is not a valid sorting field on {typeof(Creature).Name}")
        };
    }

    private IQueryable<Creature> Filter(IQueryable<Creature> queryable)
    {
        if (MinimumCRFilter != 0 || MaximumCRFilter != 30)
        {
            queryable = queryable.Where(x => x.LevelOrCR <= MaximumCRFilter && x.LevelOrCR >= MinimumCRFilter);
        }
        if (MinCreatureSizeClassFilter != CreatureSizeClass.Unknown || MaxCreatureSizeClassFilter < CreatureSizeClass.Gargantuan)
        {
            queryable = queryable.Where(x => x.Size >= MinCreatureSizeClassFilter && x.Size <= MaxCreatureSizeClassFilter);
        }
        if (CreatureAlignmentFilterSelected != CreatureAlignment.Undefined)
        {
            queryable = queryable.Where(x => x.Alignment == CreatureAlignmentFilterSelected);
        }
        if (CreatureTypeFilterSelected != CreatureType.Any)
        {
            queryable = queryable.Where(x => x.CreatureType == CreatureTypeFilterSelected);
        }
        if (!string.IsNullOrEmpty(SearchString))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(SearchString.ToLower()));
        }
        return queryable;
    }

    public async override Task ResetAsync()
    {
        SearchString = "";
        MaximumCRFilter = _maxCR; //that's as high as she goes cap'n
        MinimumCRFilter = _minCR;
        MinCreatureSizeClassFilter = CreatureSizeClass.Unknown;
        MaxCreatureSizeClassFilter = CreatureSizeClass.Gargantuan;
        CreatureAlignmentFilterSelected = CreatureAlignment.Undefined;
        CreatureTypeFilterSelected = CreatureType.Any;
        await GetPaginatedList(1, 50);
    }

    public async override Task GetPaginatedList(int pageIndex, int pageSize, DataGridColumnEventArgs? e = null)
    {

        var query = FilterAndSortQuery<Creature>(_dataService.Creatures().Include(x => x.Abilities), null, e);

        var pagedEncounters = await PaginatedList<ObservableCreature>.CreateAsync(
            query,
            (x) => new ObservableCreature((Creature)x),
            pageIndex,
            pageSize);

        PageNumber = pagedEncounters.PageIndex;
        PageCount = pagedEncounters.PageCount;
        Data = pagedEncounters;
    }
}
