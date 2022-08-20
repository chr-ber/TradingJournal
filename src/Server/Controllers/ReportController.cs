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
