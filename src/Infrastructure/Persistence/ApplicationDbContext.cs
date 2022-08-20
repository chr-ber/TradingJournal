using System.Reflection;

namespace TradingJournal.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
   private readonly IDomainEventService _domainEventService;

   public ApplicationDbContext(
       DbContextOptions<ApplicationDbContext> options,
       IDomainEventService domainEventService)
       : base(options)
   {
      _domainEventService = domainEventService;
   }

   public DbSet<User> Users { get; set; }

   public DbSet<TradingAccount> TradingAccounts { get; set; }

   public DbSet<Execution> Executions { get; set; }

   public DbSet<Trade> Trades { get; set; }

   public DbSet<Symbol> Symbols { get; set; }


   protected override void OnModelCreating(ModelBuilder builder)
   {
      // apply configurations from ./persistance/configurations folder
      builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

      base.OnModelCreating(builder);
   }

   // override SaveChangesAsny to add event dispatching
   public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
   {
      // get all unpublished domain events for the entities that are being changed
      var events = ChangeTracker.Entries<IHasDomainEvent>()
      .Select(x => x.Entity.DomainEvents)
      .SelectMany(x => x)
      .Where(domainEvent => domainEvent.IsPublished is false)
      .ToArray();

      var result = await base.SaveChangesAsync(cancellationToken);

      // dispatch all unpublished events
      await DispatchEvents(events);

      return result;
   }

   private async Task DispatchEvents(DomainEvent[] events)
   {
      foreach (var domainEvent in events)
      {
         domainEvent.IsPublished = true;
         await _domainEventService.Publish(domainEvent);
      }
   }

   protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
   {
      // set precision for all decimal properties
      configurationBuilder.Properties<decimal>()
          .HavePrecision(18, 6);
   }
}
