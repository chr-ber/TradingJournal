using TradingJournal.Application.Common.Models;
using TradingJournal.Application.CQS.Trades.Commands;
using TradingJournal.Domain.Entities;
using TradingJournal.Domain.Enums;

namespace TradingJournal.Application.Common.Interfaces;

public interface ITradeService
{
    PaginatedList<Trade> PaginatedList { get; set; }

    Task DeleteTrade(int id);

    Task LoadTrades(int pageNumber = 1, int pageSize = 10, IEnumerable<TradeStatus> includedStates = null);

    Task<Trade> GetTrade(int id);

    Task<int> GetTradeCount();

    Task<decimal> GetAverageReturn();

    Task<float> GetWinLossRatio();

    Task UpdateJournalFields(UpdateJournalingFieldsCommand command);
}