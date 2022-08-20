namespace TradingJournal.Application.Entities.Reports.Queries.GetMonthReportQuery;

public class MonthReportDto
{
   public List<MonthStatistics> MonthStatistics { get; set; } = new();

   public List<ChartSeries> MonthChart { get; set; }

   public string BestPerformingMonth { get; set; }

   public string WorstPerfromingMonth { get; set; }
}

public class MonthStatistics
{
   public string Month { get; set; }

   public double WinPercentage { get; set; }

   public double LoosePercentage { get; set; }

   public double BreakEvenPercentage { get; set; }

   public int TotalTradesTaken { get; set; }
}
