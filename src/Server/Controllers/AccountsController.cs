using TradingJournal.Application.Accounts.Commands.DeleteTradingAccount;
using TradingJournal.Application.Accounts.Commands.ChangeTradingAccountState;
using TradingJournal.Application.Accounts.Commands.CreateTradingAccount;
using TradingJournal.Application.Accounts.Queries;
using TradingJournal.Application.Common.Models;
using TradingJournal.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TradingJournal.Server.Controllers;

[Authorize]
public class AccountsController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateTradingAccountCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<TradingAccount>>> GetTradingAccountsWithPagination(
        [FromQuery] GetTradingAccountsWithPaginationQuery query)
            => await Mediator.Send(query);

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteTradingAccountCommand() { Id = id });
        return NoContent();
    }
    
    [HttpPut("set-status")]
    public async Task<IActionResult> SetStatus(SetTradingAccountStateCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
}
