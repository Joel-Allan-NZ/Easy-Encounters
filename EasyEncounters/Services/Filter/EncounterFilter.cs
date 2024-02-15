using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.ViewModels;
using Windows.Storage.AccessCache;

namespace EasyEncounters.Services.Filter;
public partial class EncounterFilter : FilterValues
{
    
    [ObservableProperty]
    private List<EncounterData> _searchSuggestions = new();

    [ObservableProperty]
    private EncounterDifficulty _minimumDifficulty;

    [ObservableProperty]
    private EncounterDifficulty _maximumDifficulty;

    [ObservableProperty]
    private int _minimumEnemiesFilter;

    [ObservableProperty]
    private int _maximumEnemiesFilter;

    readonly IList<EncounterDifficulty> _difficulties = Enum.GetValues(typeof(EncounterDifficulty)).Cast<EncounterDifficulty>().ToList();

    public IList<EncounterDifficulty> Difficulties => _difficulties;

    public ICollection<FilterCriteria<EncounterData>> GenerateFilterCriteria(string text)
    {
        List<FilterCriteria<EncounterData>> criteriaList = new List<FilterCriteria<EncounterData>>()
        {
            new(x => x.Encounter.Name, text),
            new(x => x.DifficultyDescription, MinimumDifficulty, MaximumDifficulty),
            new(x => x.Encounter.Creatures.Count, MinimumEnemiesFilter, MaximumEnemiesFilter)
        };

        return criteriaList;
    }

    public void ResetFilter()
    {
        MaximumEnemiesFilter = 1000;
        MaximumDifficulty = EncounterDifficulty.VeryDeadly;
        MinimumEnemiesFilter = 0;
        MinimumDifficulty = EncounterDifficulty.None;
    }

    public EncounterFilter()
    {
        ResetFilter();
    }
}
