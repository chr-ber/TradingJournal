namespace TradingJournal.Application.Entities.Reports.Queries.GetDailyReport;

public class DayOfWeekReportDto
{
   public List<DayOfWeekStatistics> MonthStatistics { get; set; } = new();

   public List<ChartSeries> DayOfWeekChart { get; set; }

   public string BestPerformingDay { get; set; }

   public string WorstPerfromingDay { get; set; }


}

public class DayOfWeekStatistics
{
   public string DayOfWeek { get; set; }

   public double WinPercentage { get; set; }

   public double LoosePercentage { get; set; }

   public double BreakEvenPercentage { get; set; }

   public int TotalTradesTaken { get; set; }
}
