using TradingJournal.Application.Entities.Trades.Commands.UpdateJournalingFields;
using TradingJournal.Application.Common.Models;
using TradingJournal.Domain.Entities;
using TradingJournal.Domain.Enums;

namespace TradingJournal.Application.Common.Interfaces;

public interface ITradeService
{
    PaginatedList<Trade> PaginatedList { get; set; }

    Task LoadTrades(int pageNumber = 1, int pageSize = 10, IEnumerable<TradeStatus> includedStates = null, bool hidden = false);

    Task<Trade> GetTrade(int id);

    Task<int> GetTradeCount();

    Task BatchSetVisibility(IEnumerable<int> ids, bool isHidden);

    Task<decimal> GetAverageReturn();

    Task UpdateJournalFields(UpdateJournalingFieldsCommand command);
}