using System.Linq.Expressions;
using EasyEncounters.Services.Filter;

namespace EasyEncounters.Contracts.Services;

public interface IFilteringService
{
    ICollection<T> Filter<T>(ICollection<T> toFilter, ICollection<FilterCriteria<T>> filters);// where U : IComparable

    ICollection<T> Filter<T>(ICollection<T> toFilter, FilterCriteria<T> filter);

    ICollection<T> Filter<T>(ICollection<T> toFilter, Expression<Func<T, string>> expression, string subString);

    ICollection<T> Filter<T>(ICollection<T> toFilter, Expression<Func<T, IComparable>> expression, IComparable minimum, IComparable maximum);

    ICollection<T> Filter<T>(ICollection<T> toFilter, FilterValues filterValues, string text);

    /// <summary>
    /// Get an appropriate FilterValues object for the selected type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    FilterValues GetFilterValues<T>();
}