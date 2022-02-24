using TradingJournal.Application.Common.Models;

namespace TradingJournal.Application.Common.Extensions;

public static class MappingExtensions
{
    public static Task<PaginatedList<TDest>> PaginatedListAsync<TDest>(
        this IQueryable<TDest> queryable, int selectedPage, int pageSize)
    {
        return PaginatedList<TDest>.CreateAsync(queryable, selectedPage, pageSize);
    }
}

