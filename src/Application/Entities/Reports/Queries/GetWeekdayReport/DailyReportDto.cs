using MudBlazor;

namespace TradingJournal.Application.Entities.Reports.Queries.GetDailyReport;

public class WeekdayReportDto
{
    public List<WeekDayStatistics> MonthStatistics { get; set; } = new();

    public List<ChartSeries> WeekdayChart { get; set; }

    public string BestPerformingDay { get; set; }

    public string WorstPerfromingDay { get; set; }

    
}

public class WeekDayStatistics
{
    public string DayOfWeek { get; set; }

    public double WinPercentage { get; set; }

    public double LoosePercentage { get; set; }

    public double BreakEvenPercentage { get; set; }

    public int TotalTradesTaken { get; set; }
}