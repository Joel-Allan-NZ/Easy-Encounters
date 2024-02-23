using CommunityToolkit.Mvvm.ComponentModel;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Services.Filter;

public partial class EncounterFilter : FilterValues
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
    private List<EncounterData> _searchSuggestions = new();

    public EncounterFilter()
    {
        ResetFilter();
    }

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
}