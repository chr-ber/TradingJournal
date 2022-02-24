using TradingJournal.Domain.Common;

namespace TradingJournal.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}
