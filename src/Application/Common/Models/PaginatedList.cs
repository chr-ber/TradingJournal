using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TradingJournal.Application.Common.Models;

public class PaginatedList<T>
{
    public List<T> Items { get; set; }

    public int SelectedPage { get; set; }

    public int TotalPages { get; set; }

    public int TotalCount { get; set; }

    [JsonIgnore]
    public bool HasPreviousPage => SelectedPage > 1;

    [JsonIgnore]
    public bool HasNextPage => SelectedPage < TotalPages;

   [JsonConstructor]
    public PaginatedList()
    {

    }

    public PaginatedList(List<T> items, int total, int selectedPage, int pageSize)
    {
        Items = items;
        SelectedPage = selectedPage;
        TotalCount = total;
        TotalPages = (int)Math.Ceiling(total / (double)pageSize);
    }

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int selectedPage, int pageSize)
    {
        int total = await source.CountAsync();
        List<T> items = await source.Skip((selectedPage - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PaginatedList<T>(items, total, selectedPage, pageSize);
    }
}
