using TradingJournal.Application.Common.Models;

namespace TradingJournal.Application.Common.Extensions;

public static class MappingExtensions
{
    // extension to allow any domain entity to be queried with pagination
    public static Task<PaginatedList<T1>> ToPaginatedListAsync<T1>(
        this IQueryable<T1> queryable, int selectedPage, int pageSize)
    {
        return PaginatedList<T1>.CreateAsync(queryable, selectedPage, pageSize);
    }
}