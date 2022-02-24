namespace TradingJournal.Application.Common.Interfaces;

public interface IUtilityService
{
    Task<bool> IsReadOnlyAPICredentials(string key, string secret);
}