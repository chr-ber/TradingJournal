namespace TradingJournal.Application.ClientServices;

public class TradeService : ClientServiceBase, ITradeService
{
   private readonly HttpClient _http;

   public PaginatedList<Trade> PaginatedList { get; set; }

   public TradeService(HttpClient http)
   {
      _http = http;
   }

   public async Task LoadTrades(
       int pageNumber = 1,
       int pageSize = 10,
       IEnumerable<TradeStatus> includedStates = null,
       bool hidden = false)
   {
      // create a dictionary for all query parameters
      Dictionary<string, string> query = new()
      {
         ["pageNumber"] = pageNumber.ToString(),
         ["pageSize"] = pageSize.ToString(),
         ["hidden"] = hidden.ToString(),
      };

      // if any states where specified add them
      if (includedStates != null)
         foreach (var state in includedStates)
            query["includedStates"] = ((int)state).ToString();

      // convert the query dictionary to an url encoded string
      var dictFormUrlEncoded = new FormUrlEncodedContent(query);
      var queryString = await dictFormUrlEncoded.ReadAsStringAsync();

      var json = await _http.GetStringAsync($"api/trades?{queryString}");

      PaginatedList = JsonSerializer.Deserialize<PaginatedList<Trade>>(json, _jsonOptions);
   }

   public async Task UpdateJournalFields(UpdateJournalingFieldsCommand command)
   {
      await _http.PutAsJsonAsync("api/trades/journal", command);
   }

   public async Task<Trade> GetTrade(int id)
   {
      var json = await _http.GetStringAsync($"api/trades/{id}");

      return JsonSerializer.Deserialize<Trade>(json, _jsonOptions);
   }

   public async Task<int> GetTradeCount()
   {
      var response = await _http.GetStringAsync("api/trades/count");

      return int.Parse(response);
   }

   public async Task<decimal> GetAverageReturn()
   {
      var response = await _http.GetStringAsync("api/trades/average-return");

      return decimal.Parse(response);
   }

   public async Task BulkSetVisibility(IEnumerable<int> ids, bool isHidden)
   {
      var command = new BatchSetTradeVisibilityCommand { Ids = ids, IsHidden = isHidden };
      var response = await _http.PutAsJsonAsync("api/trades/set-visibility", command);
   }
}
