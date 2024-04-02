using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Core.Contracts;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;

namespace EasyEncounters.Services.Filter;

public class FilteringService : IFilteringService
{
    private readonly IDataService _dataService;

    public FilteringService(IDataService dataService)
    {
        _dataService = dataService;        
    }
    //public ICollection<T> Filter<T>(ICollection<T> toFilter, ICollection<FilterCriteria<T>> filters) => throw new NotImplementedException();
    //public ICollection<T> Filter<T>(ICollection<T> toFilter, FilterCriteria<T> filter) => throw new NotImplementedException();
    //public ICollection<T> Filter<T>(ICollection<T> toFilter, Expression<Func<T, string>> expression, string subString) => throw new NotImplementedException();
    //public ICollection<T> Filter<T>(ICollection<T> toFilter, Expression<Func<T, IComparable>> expression, IComparable minimum, IComparable maximum) => throw new NotImplementedException();
    //public ICollection<T> Filter<T>(ICollection<T> toFilter, FilterValues<T> filterValues, string text) => throw new NotImplementedException();
    //public ICollection<T> Filter<T>(ICollection<T> toFilter, Expression<Func<T, Enum>> expression, Enum flag) => throw new NotImplementedException();
    public IFilterValues<T> GetFilterValues<T>() where T: IPersistable
    {
        if(typeof(T) == typeof(Encounter))
        {
            return (IFilterValues<T>)new EncounterFilter(_dataService);
        }
        else if(typeof(T) == typeof(Ability))
        {
            return (IFilterValues<T>)new AbilityFilter(_dataService);
        }
        else if(typeof(T) == typeof(Party))
        {
            return (IFilterValues<T>)new PartyFilter(_dataService);
        }
        else if(typeof(T) == typeof(Creature))
        {
            return (IFilterValues<T>)new CreatureFilter(_dataService);
        }
        throw new ArgumentException($"{nameof(T)} is not a currently supported IFilterValue<T> type");
    }


    public IQueryable<T> FilterAndSort<T, U>(GridFilteredValues<T> filters, IQueryable<T> queryable, U? additional, DataGridColumnEventArgs? e) where U : class
    {
        return filters.FilterAndSortQuery(queryable, additional, e);
    }
}
