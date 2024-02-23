using CommunityToolkit.Mvvm.ComponentModel;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Services.Filter;

public partial class CreatureFilter : FilterValues
{
    [ObservableProperty]
    private double _maximumCRFilter;

    [ObservableProperty]
    private double _minimumCRFilter;

    [ObservableProperty]
    private List<CreatureViewModel> _searchSuggestions = new();

    public ICollection<FilterCriteria<CreatureViewModel>> GenerateFilterCriteria(string text)
    {
        List<FilterCriteria<CreatureViewModel>> criteriaList = new List<FilterCriteria<CreatureViewModel>>()
        {
            new(x => x.Creature.LevelOrCR, MinimumCRFilter, MaximumCRFilter),
            new(x => x.Creature.Name, text)
        };

        return criteriaList;
    }

    public void ResetFilter()
    {
        MaximumCRFilter = 30; //that's as high as she goes cap'n
        MinimumCRFilter = 0;
    }
}