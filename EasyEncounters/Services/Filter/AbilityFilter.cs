using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Models;
using EasyEncounters.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace EasyEncounters.Services.Filter;
/// <summary>
/// Wrapper for Ability Properties of interest for filtering, including convenience lists of enum values
/// </summary>
public partial class AbilityFilter : FilterValues
{
    readonly IList<SpellLevel> _spellLevels = Enum.GetValues(typeof(SpellLevel)).Cast<SpellLevel>().ToList();
    readonly IList<MagicSchool> _magicSchools = Enum.GetValues(typeof(MagicSchool)).Cast<MagicSchool>().ToList();
    readonly IList<DamageType> _damageTypes = Enum.GetValues(typeof(DamageType)).Cast<DamageType>().ToList();
    readonly IList<ResolutionType> _resolutionTypes = Enum.GetValues(typeof(ResolutionType)).Cast<ResolutionType>().ToList();
    readonly IList<ThreeStateBoolean> _concentrationStates = Enum.GetValues(typeof(ThreeStateBoolean)).Cast<ThreeStateBoolean>().ToList();

    public IList<SpellLevel> SpellLevels => _spellLevels;
    public IList<MagicSchool> MagicSchools => _magicSchools;
    public IList<DamageType> DamageTypes => _damageTypes;
    public IList<ResolutionType> ResolutionTypes => _resolutionTypes;
    public IList<ThreeStateBoolean> ConcentrationStates => _concentrationStates;

    [ObservableProperty]
    private SpellLevel _minimumSpellLevelFilter;

    [ObservableProperty]
    private SpellLevel _maximumSpellLevelFilter;

    [ObservableProperty]
    private MagicSchool _spellSchoolFilterSelected;

    [ObservableProperty]
    private DamageType _damageTypeFilterSelected;

    [ObservableProperty]
    private ResolutionType _resolutionTypeFilterSelected;

    [ObservableProperty]
    private ThreeStateBoolean _concentrationFilterSelected;

    [ObservableProperty]
    private List<AbilityViewModel> _searchSuggestions = new();

    public ICollection<FilterCriteria<AbilityViewModel>> GenerateFilterCriteria(string text)
    {

        List<FilterCriteria<AbilityViewModel>> criteria = new()
            {
            new FilterCriteria<AbilityViewModel>(x => x.Ability.SpellLevel, MinimumSpellLevelFilter, MaximumSpellLevelFilter),
            //new FilterCriteria<AbilityViewModel>(x => x.Ability.Name, text),
            };
        if (!String.IsNullOrEmpty(text))
            criteria.Add(new FilterCriteria<AbilityViewModel>(x => x.Ability.Name, text));
        if (ConcentrationFilterSelected == ThreeStateBoolean.False)
            criteria.Add(new FilterCriteria<AbilityViewModel>(x => x.Ability.Concentration, false, false));
        if (ConcentrationFilterSelected == ThreeStateBoolean.True)
            criteria.Add(new FilterCriteria<AbilityViewModel>(x => x.Ability.Concentration, true, true));
        if (ResolutionTypeFilterSelected != ResolutionType.Undefined)
            criteria.Add(new FilterCriteria<AbilityViewModel>(x => x.Ability.Resolution, ResolutionTypeFilterSelected, ResolutionTypeFilterSelected));
        if (SpellSchoolFilterSelected != MagicSchool.None)
            criteria.Add(new FilterCriteria<AbilityViewModel>(x => x.Ability.MagicSchool, SpellSchoolFilterSelected, SpellSchoolFilterSelected));
        if (DamageTypeFilterSelected != DamageType.Untyped)
            criteria.Add(new FilterCriteria<AbilityViewModel>(x => x.Ability.DamageTypes, DamageTypeFilterSelected, DamageTypeFilterSelected));

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
    }

    public AbilityFilter()
    {
        ResetFilter();
    }
}
