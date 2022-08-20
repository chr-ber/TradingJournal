namespace TradingJournal.Application.Common.Interfaces;

public interface ITradingAccountService
{
   PaginatedList<TradingAccount> List { get; set; }

   Task DeleteTradingAccount(int id);

   Task SetStatus(int id, TradingAccountStatus status);

   Task LoadTradingAccounts(int pageNumber = 1, int pageSize = 10);

   Task<ServiceResponse<int>> AddTradingAccount(CreateTradingAccountCommand command);
}
