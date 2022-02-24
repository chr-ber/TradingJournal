using TradingJournal.Domain.Common;
using TradingJournal.Domain.Enums;

namespace TradingJournal.Domain.Entities;

public class TradingAccount : IHasDomainEvent
{
    public int Id { get; set; }

    public string Name { get; set; }

    public TradingAccountStatus Status { get; set; }

    public int UserId { get; set; }

    public User User { get; set; }

    public string APIKey { get; set; }

    public string APISecret { get; set; }

    public List<DomainEvent> DomainEvents { get; set; } = new();
}