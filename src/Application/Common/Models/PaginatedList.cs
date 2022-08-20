namespace TradingJournal.Application.Common.Models;

public class PaginatedList<T>
{
   public List<T> Items { get; init; }

   public int SelectedPage { get; init; }

   public int TotalPages { get; init; }

   public int TotalCount { get; init; }

   [JsonIgnore]
   public bool HasPreviousPage => SelectedPage > 1;

   [JsonIgnore]
   public bool HasNextPage => SelectedPage < TotalPages;

   // constructor to be used for deserialization from json
   [JsonConstructor]
   public PaginatedList() { }

   public PaginatedList(List<T> items, int total, int selectedPage, int pageSize)
   {
      Items = items;
      SelectedPage = selectedPage;
      TotalCount = total;
      TotalPages = (int)Math.Ceiling(total / (double)pageSize);
   }

   public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int selectedPage, int pageSize)
   {
      var total = await source.CountAsync();

      // finalize the query and execute it
      var items = await source
          .Skip((selectedPage - 1) * pageSize)
          .Take(pageSize)
          .ToListAsync();

      return new PaginatedList<T>(items, total, selectedPage, pageSize);
   }
}
