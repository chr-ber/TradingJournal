namespace TradingJournal.Server.Controllers;

[Authorize]
public class TradesController : ApiControllerBase
{

   [HttpGet]
   public async Task<ActionResult<PaginatedList<Trade>>> GetTradesWithPagination(
       [FromQuery] GetPaginatedTradesQuery query)
   {
      return await Mediator.Send(query);
   }

   [HttpGet("{id}")]
   public async Task<ActionResult<Trade>> GetTrade(int id)
   {
      return await Mediator.Send(new GetTradeByIdQuery { Id = id });
   }

   [HttpGet("count")]
   public async Task<ActionResult<int>> GetTradeCount()
   {
      return await Mediator.Send(new GetTradeCountQuery());
   }

   [HttpGet("average-return")]
   public async Task<ActionResult<decimal>> GetAverageReturn()
   {
      return await Mediator.Send(new GetAverageTradeReturnQuery());
   }

   [HttpPut("journal")]
   public async Task<IActionResult> SetJournalingFields(UpdateJournalingFieldsCommand command)
   {
      await Mediator.Send(command);
      return NoContent();
   }

   [HttpPut("set-visibility")]
   public async Task<IActionResult> SetVisibility(BatchSetTradeVisibilityCommand command)
   {
      await Mediator.Send(command);
      return NoContent();
   }
}
