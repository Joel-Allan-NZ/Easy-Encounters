using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using Windows.ApplicationModel.Contacts;

namespace EasyEncounters.Services.Filter;

public partial class AbilityFilter : GridFilteredValues<Ability>
{
    private readonly IList<ThreeStateBoolean> _concentrationStates = Enum.GetValues(typeof(ThreeStateBoolean)).Cast<ThreeStateBoolean>().ToList();
    private readonly IList<DamageType> _damageTypes = Enum.GetValues(typeof(DamageType)).Cast<DamageType>().ToList();
    private readonly IList<MagicSchool> _magicSchools = Enum.GetValues(typeof(MagicSchool)).Cast<MagicSchool>().ToList();
    private readonly IList<ResolutionType> _resolutionTypes = Enum.GetValues(typeof(ResolutionType)).Cast<ResolutionType>().ToList();
    private readonly IList<SpellLevel> _spellLevels = Enum.GetValues(typeof(SpellLevel)).Cast<SpellLevel>().ToList();
    private readonly IDataService _dataService;
    private readonly IList<ActionSpeed> _actionSpeeds = Enum.GetValues(typeof(ActionSpeed)).Cast<ActionSpeed>().ToList();
    private readonly string _safeTag = "AbilityName";

    [ObservableProperty]
    private List<Ability> _data;

    [ObservableProperty]
    private ThreeStateBoolean _concentrationFilterSelected;

    [ObservableProperty]
    private DamageType _damageTypeFilterSelected;

    [ObservableProperty]
    private SpellLevel _maximumSpellLevelFilter;

    [ObservableProperty]
    private SpellLevel _minimumSpellLevelFilter;

    [ObservableProperty]
    private ResolutionType _resolutionTypeFilterSelected;

    [ObservableProperty]
    private List<Ability> _searchSuggestions = new();

    [ObservableProperty]
    private MagicSchool _spellSchoolFilterSelected;

    [ObservableProperty]
    private ActionSpeed _actionSpeedFilterSelected;

    //[ObservableProperty]
    //private string _searchString;

    public IList<ThreeStateBoolean> ConcentrationStates => _concentrationStates;
    public IList<DamageType> DamageTypes => _damageTypes;
    public IList<MagicSchool> MagicSchools => _magicSchools;
    public IList<ResolutionType> ResolutionTypes => _resolutionTypes;
    public IList<SpellLevel> SpellLevels => _spellLevels;
    public IList<ActionSpeed> ActionSpeeds => _actionSpeeds;

    public AbilityFilter(IDataService dataService)
    {
        _sortAscending = true;
        Data = new();
        _sortTag = _safeTag;
        _dataService = dataService;
        SearchString = "";
        //Reset();
        _namesCache = (from a in _dataService.Abilities().Where(x => x.SpellLevel != SpellLevel.NotASpell) select a.Name).ToList();
    }
    public override IQueryable<Ability> FilterAndSortQuery<U>(IQueryable<Ability> queryable, U? additionalData, DataGridColumnEventArgs? e = null) where U : class
    {
        HandleDataGrid(e, _safeTag);

        queryable = Filter(queryable);

        return _sortTag switch
        {
            "AbilityName" => _sortAscending ? queryable.OrderBy(x => x.Name) : queryable.OrderByDescending(x => x.Name),
            "AbilityLevel" => _sortAscending ? queryable.OrderBy(x => x.SpellLevel) : queryable.OrderByDescending(x => x.SpellLevel),
            "AbilityDamageType" => _sortAscending ? queryable.OrderBy(x => x.DamageTypes) : queryable.OrderByDescending(x => x.DamageTypes),
            "AbilityResolutionType" => _sortAscending ? queryable.OrderBy(x => x.Resolution) : queryable.OrderByDescending(x => x.Resolution),
            "AbilityConcentration" => _sortAscending ? queryable.OrderBy(x => x.Concentration) : queryable.OrderByDescending(x => x.Concentration),
            "AbilityResolutionStat" => _sortAscending ? queryable.OrderBy(x => x.ResolutionStat) : queryable.OrderByDescending(x => x.ResolutionStat),
            "AbilitySchool" => _sortAscending ? queryable.OrderBy(x => x.MagicSchool) : queryable.OrderByDescending(x => x.MagicSchool),
            "AbilityActionSpeed" => _sortAscending ? queryable.OrderBy(x=> x.ActionSpeed) : queryable.OrderByDescending(x => x.ActionSpeed),
            _ => throw new ArgumentException($"{_sortTag} is not a valid sorting field on {typeof(Ability).Name}")
        };
    }

    private IQueryable<Ability> Filter(IQueryable<Ability> queryable)
    {
        queryable = queryable.Where(x => x.SpellLevel >= MinimumSpellLevelFilter && x.SpellLevel <= MaximumSpellLevelFilter);

        if(ConcentrationFilterSelected == ThreeStateBoolean.True)
        {
            queryable = queryable.Where(x => x.Concentration);
        }
        else if(ConcentrationFilterSelected == ThreeStateBoolean.False)
        {
            queryable = queryable.Where(x => !x.Concentration);
        }

        if(DamageTypeFilterSelected != DamageType.None)
        {
            queryable = queryable.Where(x => (x.DamageTypes & DamageTypeFilterSelected) != 0); 
        }

        if(ResolutionTypeFilterSelected != ResolutionType.Undefined)
        {
            queryable = queryable.Where(x => x.Resolution == ResolutionTypeFilterSelected);
        }

        if(SpellSchoolFilterSelected != MagicSchool.None)
        {
            queryable = queryable.Where(x => x.MagicSchool == SpellSchoolFilterSelected);
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
        MaximumSpellLevelFilter = SpellLevel.LevelNine;
        ConcentrationFilterSelected = ThreeStateBoolean.Either;
        MinimumSpellLevelFilter = SpellLevel.Cantrip;
        DamageTypeFilterSelected = DamageType.None;
        ResolutionTypeFilterSelected = ResolutionType.Undefined;
        SpellSchoolFilterSelected = MagicSchool.None;
        ActionSpeedFilterSelected = ActionSpeed.None;
        await GetPaginatedList(1, 50);
    }
    //public override void ResetFilter()
    //{
    //    MaximumSpellLevelFilter = SpellLevel.LevelNine;
    //    ConcentrationFilterSelected = ThreeStateBoolean.Either;
    //    MinimumSpellLevelFilter = SpellLevel.Cantrip;
    //    DamageTypeFilterSelected = DamageType.Untyped;
    //    ResolutionTypeFilterSelected = ResolutionType.Undefined;
    //    SpellSchoolFilterSelected = MagicSchool.None;
    //    ActionSpeedFilterSelected = ActionSpeed.None;
    //}

    public async override Task GetPaginatedList(int pageIndex, int pageSize, DataGridColumnEventArgs? e = null)
    {
        var query = FilterAndSortQuery<Creature>(_dataService.Abilities().Where(x => x.SpellLevel != Core.Models.Enums.SpellLevel.NotASpell), null, e);

        var pagedEncounters = await PaginatedList<Ability>.CreateAsync(
            query,
            (x) => (Ability)x,
            pageIndex,
            pageSize);

        PageNumber = pagedEncounters.PageIndex;
        PageCount = pagedEncounters.PageCount;
        Data = pagedEncounters;
    }
}
