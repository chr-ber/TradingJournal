using Microsoft.EntityFrameworkCore;
using TradingJournal.Domain.Entities;

namespace TradingJournal.Application.Common.Interfaces;

public interface IApplicationDbContext : IDisposable
{
    DbSet<User> Users { get; set; }

    DbSet<TradingAccount> TradingAccounts { get; set; }

    DbSet<Execution> Executions { get; set; }

    DbSet<Symbol> Contracts { get; set; }

    DbSet<Trade> Trades { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}