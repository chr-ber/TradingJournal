using TradingJournal.Domain.Common;
using TradingJournal.Domain.Entities;

namespace TradingJournal.Domain.Events;

public class TradingAccountCreatedEvent : DomainEvent
{
    public TradingAccount Account;

    public TradingAccountCreatedEvent(TradingAccount account)
    {
        Account = account;
    }
}

