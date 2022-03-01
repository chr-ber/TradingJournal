using TradingJournal.Application.CQS.Reports.Queries;
using TradingJournal.Application.CQS.Reports.Queries.GetDailyReport;

namespace TradingJournal.Application.Common.Interfaces;

public interface IReportService
{
    Task<DailyReportDto> GetDailyReport();
    Task<DailyReportDto> GetMonthlyReport();
    Task<DailyReportDto> GetWeeklyReport();
}
