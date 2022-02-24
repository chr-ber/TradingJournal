using TradingJournal.Domain.Enums;

namespace TradingJournal.Domain.Entities;

public class Trade
{
    public int Id { get; set; }

    public TradeSide Side { get; set; }

    public Symbol Symbol { get; set; }

    public int SymbolId { get; set; }

    public TradingAccount TradingAccount { get; set; }

    public int TradingAccountId { get; set; }

    public List<Execution> Executions { get; set; } = new();

    public decimal Size { get; set; }

    public decimal Position { get; set; }

    public decimal Cost { get; set; }

    public decimal AverageEntryPrice { get; set; }

    public decimal AverageExitPrice { get; set; }

    public decimal Return { get; set; }

    public decimal ReturnPercentage { get; set; }

    public decimal NetReturn { get; set; }

    public TradeResult Result { get; set; }

    public int Confluences { get; set; }

    public string Notes { get; set; }
}