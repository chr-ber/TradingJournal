using TradingJournal.Application.Entities.Reports.Queries.GetDailyReport;
using TradingJournal.Application.Entities.Reports.Queries.GetMonthReportQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TradingJournal.Server.Controllers;

[Authorize]
public class ReportsController : ApiControllerBase
{

    [HttpGet("weekday")]
    public async Task<ActionResult<WeekdayReportDto>> GetWeeddayReport()
    {
        return await Mediator.Send(new GetWeekdayReportQuery());
    }

    [HttpGet("month")]
    public async Task<ActionResult<MonthReportDto>> GetMonthReport()
    {
        return await Mediator.Send(new GetMonthReportQuery());
    }
}
