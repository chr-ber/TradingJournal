namespace TradingJournal.Application.Common.Interfaces;

public interface IReportService
{
   Task<DayOfWeekReportDto> GetDayOfWeekReport();
   Task<MonthReportDto> GetMonthReport();
}
