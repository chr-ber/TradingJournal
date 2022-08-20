namespace TradingJournal.Application.Common.Interfaces;

public interface ICurrentUserService
{
   public Task<User> GetUser();

   public Task<int> GetUserId();
}
