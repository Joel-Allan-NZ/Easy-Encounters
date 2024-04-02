using System.Linq.Expressions;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Core.Contracts;
using EasyEncounters.Services.Filter;

namespace EasyEncounters.Contracts.Services;

public interface IFilteringService
{
    //ICollection<T> Filter<T>(ICollection<T> toFilter, ICollection<FilterCriteria<T>> filters);// where U : IComparable

    //ICollection<T> Filter<T>(ICollection<T> toFilter, FilterCriteria<T> filter);

    //ICollection<T> Filter<T>(ICollection<T> toFilter, Expression<Func<T, string>> expression, string subString);

    //ICollection<T> Filter<T>(ICollection<T> toFilter, Expression<Func<T, IComparable>> expression, IComparable minimum, IComparable maximum);

    //ICollection<T> Filter<T>(ICollection<T> toFilter, FilterValues<T> filterValues, string text);

    //ICollection<T> Filter<T>(ICollection<T> toFilter, Expression<Func<T, Enum>> expression, Enum flag);

    /// <summary>
    /// Get an appropriate FilterValues object for the selected type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    IFilterValues<T> GetFilterValues<T>() where T : IPersistable;

    IQueryable<T> FilterAndSort<T, U>(GridFilteredValues<T> filters, IQueryable<T> queryable, U? additional, DataGridColumnEventArgs? e) where U : class;
}