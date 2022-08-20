namespace TradingJournal.Application.Common.Interfaces;

public interface IApplicationDbContext : IDisposable
{
   DbSet<User> Users { get; set; }

   DbSet<TradingAccount> TradingAccounts { get; set; }

   DbSet<Execution> Executions { get; set; }

   DbSet<Symbol> Symbols { get; set; }

   DbSet<Trade> Trades { get; set; }

   Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
