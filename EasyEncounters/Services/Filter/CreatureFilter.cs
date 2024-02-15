using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Services.Filter;
public partial class CreatureFilter : FilterValues
{

    [ObservableProperty]
    private List<CreatureViewModel> _searchSuggestions = new();

    [ObservableProperty]
    private double _minimumCRFilter;

    [ObservableProperty]
    private double _maximumCRFilter;

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
