using TradingJournal.Domain.Entities;

namespace TradingJournal.Application.Common.Interfaces;

public interface IAccountSynchronizationService
{
    Task ActivateAccount(TradingAccount account);
    Task DeactivateAccount(TradingAccount account);
}