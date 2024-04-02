using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EasyEncounters.Models;

public class PaginatedList<T> : List<T>
{
    private PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageCount = (int)Math.Ceiling(count / (double)pageSize);
        AddRange(items);
    }

    public int PageCount
    {
        get; private set;
    }

    public int PageIndex
    {
        get; private set;
    }

    public static async Task<PaginatedList<T>> CreateAsync(
    IQueryable<object> source,
    Func<object, T> conversionMethod,
    int pageIndex,
    int pageSize)
    {

        int count = await source.CountAsync();
        List<T> items = await source
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .Select(x => conversionMethod(x))
                                    .ToListAsync();

        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}