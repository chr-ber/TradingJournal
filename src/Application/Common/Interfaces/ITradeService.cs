namespace TradingJournal.Application.Common.Interfaces;

public interface ITradeService
{
   PaginatedList<Trade> PaginatedList { get; set; }

   Task LoadTrades(int pageNumber = 1, int pageSize = 10, IEnumerable<TradeStatus> includedStates = null, bool hidden = false);

   Task UpdateJournalFields(UpdateJournalingFieldsCommand command);

   Task BulkSetVisibility(IEnumerable<int> ids, bool isHidden);
   Task<decimal> GetAverageReturn();

   Task<Trade> GetTrade(int id);

   Task<int> GetTradeCount();

}
