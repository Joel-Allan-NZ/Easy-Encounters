using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Services.Filter;

public partial class CreatureFilter : FilterValues
{
    private readonly IList<CreatureType> _creatureTypes = Enum.GetValues<CreatureType>().Cast<CreatureType>().ToList();
    private readonly IList<CreatureSizeClass> _creatureSizeClasses = Enum.GetValues<CreatureSizeClass>().Cast<CreatureSizeClass>().ToList();
    private readonly IList<CreatureAlignment> _alignments = Enum.GetValues<CreatureAlignment>().Cast<CreatureAlignment>().ToList();

    [ObservableProperty]
    private double _maximumCRFilter;

    [ObservableProperty]
    private double _minimumCRFilter;

    [ObservableProperty]
    private List<ObservableCreature> _searchSuggestions = new();

    [ObservableProperty]
    private CreatureType _creatureTypeFilterSelected;

    [ObservableProperty]
    private CreatureSizeClass _minCreatureSizeClassFilter;

    [ObservableProperty]
    private CreatureSizeClass _maxCreatureSizeClassFilter;

    [ObservableProperty]
    private CreatureAlignment _creatureAlignmentFilterSelected;

    public IList<CreatureAlignment> Alignments => _alignments;
    public IList<CreatureSizeClass> SizeClasses => _creatureSizeClasses;
    public IList<CreatureType> CreatureTypes => _creatureTypes;

    public ICollection<FilterCriteria<ObservableCreature>> GenerateFilterCriteria(string text)
    {
        List<FilterCriteria<ObservableCreature>> criteriaList = new List<FilterCriteria<ObservableCreature>>()
        {
            new(x => x.Creature.LevelOrCR, MinimumCRFilter, MaximumCRFilter),
            new(x => x.Creature.Size, MinCreatureSizeClassFilter, MaxCreatureSizeClassFilter),
            new(x => x.Creature.Alignment, CreatureAlignmentFilterSelected),
            new(x => x.Creature.CreatureType, CreatureTypeFilterSelected),
            new(x => x.Creature.Name, text)
        };

        return criteriaList;
    }
    public CreatureFilter()
    {
        ResetFilter();
    }

    public void ResetFilter()
    {
        MaximumCRFilter = 30; //that's as high as she goes cap'n
        MinimumCRFilter = 0;
        MinCreatureSizeClassFilter = CreatureSizeClass.Unknown;
        MaxCreatureSizeClassFilter = CreatureSizeClass.Gargantuan;
        CreatureAlignmentFilterSelected = CreatureAlignment.Undefined;
        CreatureTypeFilterSelected = CreatureType.Any;
    }

    public void SortCollection(ObservableCollection<ObservableCreature> collection, DataGridColumnEventArgs e)
    {
        var sortDirection = e.Column.SortDirection == DataGridSortDirection.Ascending;

        var tagString = e.Column.Tag.ToString();

        Func<ObservableCreature, object> predicate = tagString switch
        {
            "CreatureName" => new(x => x.Creature.Name),
            "CreatureCR" => new(x => x.Creature.LevelOrCR),
            "CreatureAlignment" => new(x => x.Creature.Alignment),
            "CreatureSize" => new(x => x.Creature.Size),
            "CreatureType" => new(x => x.Creature.CreatureType),
            _ => throw new Exception("Not a valid tag name")
        };

        SortByPredicate(collection, predicate, sortDirection);


        if (e.Column.SortDirection == null || !sortDirection)
        {
            e.Column.SortDirection = DataGridSortDirection.Ascending;
        }
        else
        {
            e.Column.SortDirection = DataGridSortDirection.Descending;
        }
    }


}