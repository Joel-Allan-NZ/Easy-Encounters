using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Models;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Services.Filter;

/// <summary>
/// Wrapper for Ability Properties of interest for filtering, including convenience lists of enum values
/// </summary>
public partial class AbilityFilter : FilterValues
{
    private readonly IList<ThreeStateBoolean> _concentrationStates = Enum.GetValues(typeof(ThreeStateBoolean)).Cast<ThreeStateBoolean>().ToList();
    private readonly IList<DamageType> _damageTypes = Enum.GetValues(typeof(DamageType)).Cast<DamageType>().ToList();
    private readonly IList<MagicSchool> _magicSchools = Enum.GetValues(typeof(MagicSchool)).Cast<MagicSchool>().ToList();
    private readonly IList<ResolutionType> _resolutionTypes = Enum.GetValues(typeof(ResolutionType)).Cast<ResolutionType>().ToList();
    private readonly IList<SpellLevel> _spellLevels = Enum.GetValues(typeof(SpellLevel)).Cast<SpellLevel>().ToList();
    private readonly IList<ActionSpeed> _actionSpeeds = Enum.GetValues(typeof(ActionSpeed)).Cast<ActionSpeed>().ToList();

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

    public AbilityFilter()
    {
        ResetFilter();
    }

    public IList<ThreeStateBoolean> ConcentrationStates => _concentrationStates;
    public IList<DamageType> DamageTypes => _damageTypes;
    public IList<MagicSchool> MagicSchools => _magicSchools;
    public IList<ResolutionType> ResolutionTypes => _resolutionTypes;
    public IList<SpellLevel> SpellLevels => _spellLevels;
    public IList<ActionSpeed> ActionSpeeds => _actionSpeeds;

    public ICollection<FilterCriteria<Ability>> GenerateFilterCriteria(string text)
    {
        List<FilterCriteria<Ability>> criteria = new()
            {
            new FilterCriteria<Ability>(x => x.SpellLevel, MinimumSpellLevelFilter, MaximumSpellLevelFilter),
            };
        if (!String.IsNullOrEmpty(text))
            criteria.Add(new FilterCriteria<Ability>(x => x.Name, text));
        if (ConcentrationFilterSelected == ThreeStateBoolean.False)
            criteria.Add(new FilterCriteria<Ability>(x => x.Concentration, false, false));
        if (ConcentrationFilterSelected == ThreeStateBoolean.True)
            criteria.Add(new FilterCriteria<Ability>(x => x.Concentration, true, true));
        if (ResolutionTypeFilterSelected != ResolutionType.Undefined)
            criteria.Add(new FilterCriteria<Ability>(x => x.Resolution, ResolutionTypeFilterSelected, ResolutionTypeFilterSelected));
        if (SpellSchoolFilterSelected != MagicSchool.None)
            criteria.Add(new FilterCriteria<Ability>(x => x.MagicSchool, SpellSchoolFilterSelected, SpellSchoolFilterSelected));
        if (DamageTypeFilterSelected != DamageType.Untyped)
            criteria.Add(new FilterCriteria<Ability>(x => x.DamageTypes, DamageTypeFilterSelected, DamageTypeFilterSelected));
        if (ActionSpeedFilterSelected != ActionSpeed.None)
            criteria.Add(new FilterCriteria<Ability>(x => x.ActionSpeed, ActionSpeedFilterSelected, ActionSpeedFilterSelected));

        return criteria;
    }

    public void ResetFilter()
    {
        MaximumSpellLevelFilter = SpellLevel.LevelNine;
        ConcentrationFilterSelected = ThreeStateBoolean.Either;
        MinimumSpellLevelFilter = SpellLevel.Cantrip;
        DamageTypeFilterSelected = DamageType.Untyped;
        ResolutionTypeFilterSelected = ResolutionType.Undefined;
        SpellSchoolFilterSelected = MagicSchool.None;
        ActionSpeedFilterSelected = ActionSpeed.None;
    }

    public void SortCollection(ObservableCollection<Ability> collection, DataGridColumnEventArgs e)
    {
        var sortDirection = e.Column.SortDirection == DataGridSortDirection.Ascending;

        var tagString = e.Column.Tag.ToString();

        Func<Ability, object> predicate = tagString switch
        {
            "AbilityName" => new(x=> x.Name),
            "AbilityLevel" => new(x=>x.SpellLevel),
            "AbilityDamageType" => new(x => x.DamageTypes),
            "AbilityResolutionType" => new(x => x.Resolution),
            "AbilityConcentration" => new(x => x.Concentration),
            "AbilityResolutionStat" => new(x => x.SaveType),
            "AbilitySchool" => new(x => x.MagicSchool),
            "AbilityActionSpeed" => new(x => x.ActionSpeed),
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