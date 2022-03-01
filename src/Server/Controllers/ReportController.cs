using TradingJournal.Application.CQS.Reports.Queries.GetDailyReport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TradingJournal.Server.Controllers;

[Authorize]
public class ReportsController : ApiControllerBase
{

    [HttpGet("daily")]
    public async Task<ActionResult<DailyReportDto>> GetDailyReport()
    {
        return await Mediator.Send(new GetDailyReportQuery());
    }
}
