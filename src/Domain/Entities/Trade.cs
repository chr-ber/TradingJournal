using TradingJournal.Domain.Enums;

namespace TradingJournal.Domain.Entities;

public class Trade
{
    public int Id { get; set; }

    public TradeSide Side { get; set; }

    public virtual Symbol Symbol { get; set; }

    public int SymbolId { get; set; }

    public virtual TradingAccount TradingAccount { get; set; }

    public int TradingAccountId { get; set; }

    public virtual List<Execution> Executions { get; set; } = new();

    public decimal Size { get; set; }

    public decimal Position { get; set; }

    public decimal Cost { get; set; }

    public decimal AverageEntryPrice { get; set; }

    public decimal AverageExitPrice { get; set; }

    public decimal Return { get; set; }

    public decimal NetReturn { get; set; }

    public TradeStatus Status { get; set; }

    public int Confluences { get; set; }

    public string Notes { get; set; }

    public DateTime OpenedAt { get; set; }

    public DateTime ClosedAt { get; set; }

    public bool IsHidden { get; set; } = false;
}