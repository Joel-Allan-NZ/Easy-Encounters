using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Services;

namespace EasyEncounters.Contracts.Services;
public interface IFilteringService
{
    ICollection<T> Filter<T>(ICollection<T> toFilter, ICollection<FilterCriteria<T>> filters);// where U : IComparable

    ICollection<T> Filter<T>(ICollection<T> toFilter, FilterCriteria<T> filter);

    ICollection<T> Filter<T>(ICollection<T> toFilter, Expression<Func<T, string>> expression, string subString);

    ICollection<T> Filter<T>(ICollection<T> toFilter, Expression<Func<T, IComparable>> expression, IComparable minimum, IComparable maximum);
}
