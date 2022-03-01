using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingJournal.Domain.Entities;
using TradingJournal.Domain.Enums;

namespace TradingJournal.Application.Trades.Queries;
public class TradeDto
{
    public int Id { get; set; }

    public TradeSide Side { get; set; }

    public virtual Symbol Symbol { get; set; }

    public string AccountName { get; set; }

    public virtual List<Execution> Executions { get; set; } = new();

    public decimal Size { get; set; }

    public decimal Position { get; set; }

    public decimal Cost { get; set; }

    public decimal AverageEntryPrice { get; set; }

    public decimal AverageExitPrice { get; set; }

    public decimal Return { get; set; }

    public decimal ReturnPercentage { get; set; }

    public decimal NetReturn { get; set; }

    public TradeStatus Result { get; set; }

    public int Confluences { get; set; }

    public string Notes { get; set; }
}
