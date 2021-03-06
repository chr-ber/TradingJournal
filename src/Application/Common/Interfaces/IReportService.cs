using TradingJournal.Application.Entities.Reports.Queries;
using TradingJournal.Application.Entities.Reports.Queries.GetDailyReport;
using TradingJournal.Application.Entities.Reports.Queries.GetMonthReportQuery;

namespace TradingJournal.Application.Common.Interfaces;

public interface IReportService
{
    Task<DayOfWeekReportDto> GetDayOfWeekReport();
    Task<MonthReportDto> GetMonthReport();
}
