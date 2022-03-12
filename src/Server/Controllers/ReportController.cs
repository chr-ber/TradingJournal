using TradingJournal.Application.Entities.Reports.Queries.GetDailyReport;
using TradingJournal.Application.Entities.Reports.Queries.GetMonthReportQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TradingJournal.Server.Controllers;

[Authorize]
public class ReportsController : ApiControllerBase
{

    [HttpGet("dayofweek")]
    public async Task<ActionResult<DayOfWeekReportDto>> GetWeeddayReport()
    {
        return await Mediator.Send(new GetDayOfWeekReportQuery());
    }

    [HttpGet("month")]
    public async Task<ActionResult<MonthReportDto>> GetMonthReport()
    {
        return await Mediator.Send(new GetMonthReportQuery());
    }
}
