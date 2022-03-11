using TradingJournal.Domain.Enums;

namespace TradingJournal.Domain.Entities;

public class Execution
{
    public int Id { get; set; }

    public TradeAction Action { get; set; }

    public Trade Trade { get; set; }

    public int TradeId { get; set; }

    public DateTime ExecutedAt { get; set; }

    public decimal Size { get; set; }

    public decimal Position { get; set; }

    public decimal Price { get; set; }

    public decimal Value { get; set; }

    public decimal Fee { get; set; }

    public decimal Return { get; set; }

    public decimal NetReturn { get; set; }

    public TradeDirection Direction { get; set; }
}
