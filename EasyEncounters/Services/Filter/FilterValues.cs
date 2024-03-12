using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace EasyEncounters.Services.Filter;

public abstract class FilterValues : ObservableObject
{
    //public abstract ICollection<FilterCriteria<T>> GenerateFilterCriteria<T>(string text);
    protected static void SortByPredicate<T, U>(ObservableCollection<T> collection, Func<T, U> expression, bool ascending)
    {
        IEnumerable<T> tmp = (ascending) ? collection.OrderBy(expression).ToList() : collection.OrderByDescending(expression).ToList();

        collection.Clear();
        foreach (var item in tmp)
            collection.Add(item);
    }
}