namespace TradingJournal.Application.Common.Interfaces;

public interface IApiUtilityService
{
   Task<bool> IsReadOnlyAPICredentials(string key, string secret);
}
