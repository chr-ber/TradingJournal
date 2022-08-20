namespace TradingJournal.Infrastructure.Persistence.Configurations;

public class ExecutionConfiguration : IEntityTypeConfiguration<Execution>
{
   public void Configure(EntityTypeBuilder<Execution> builder)
   {
      builder.HasKey(x => x.Id);

      // all decimals are configured via ApplicationDbContext.ConfigureConventions()
   }
}
