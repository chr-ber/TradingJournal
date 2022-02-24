using TradingJournal.Application.Common.Models;
using TradingJournal.Application.TradingAccounts.Commands.CreateTradingAccount;
using TradingJournal.Domain.Entities;
using TradingJournal.Domain.Enums;

namespace TradingJournal.Application.Common.Interfaces;
public interface ITradingAccountService
{
    PaginatedList<TradingAccount> List { get; set; }

    Task LoadTradingAccounts(int pageNumber = 1, int pageSize = 10);

    Task<ServiceResponse<int>> AddTradingAccount(CreateTradingAccountCommand command);

    Task DeleteTradingAccount(int id);

    Task SetStatus(int id, TradingAccountStatus status);
}
