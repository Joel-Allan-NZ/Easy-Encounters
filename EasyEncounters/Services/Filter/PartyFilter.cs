using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Models;

namespace EasyEncounters.Services.Filter;
public partial class PartyFilter : FilterValues
{
    [ObservableProperty]
    private List<Party> _searchSuggestions = new();

    public PartyFilter()
    {
        ResetFilter();
    }

    public void ResetFilter()
    {
        
    }

    public void SortCollection(ObservableCollection<Party> collection, DataGridColumnEventArgs e)
    {
        var sortDirection = e.Column.SortDirection == DataGridSortDirection.Ascending;

        var tagString = e.Column.Tag.ToString();

        Func<Party, object> predicate = tagString switch
        {
            "PartyName" => new(x => x.Name),
            "PartyMemberCount" => new(x => x.Members.Count),
            "PartyCampaign" => new(x => x.Campaign?.Name ?? ""),
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

    public ICollection<FilterCriteria<Party>> GenerateFilterCriteria(string text)
    {
        List<FilterCriteria<Party>> criteria = new();
        if (!String.IsNullOrEmpty(text))
            criteria.Add(new(x => x.Name, text));


        return criteria;
    }
}
