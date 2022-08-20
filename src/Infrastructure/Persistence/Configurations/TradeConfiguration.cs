namespace TradingJournal.Infrastructure.Server.Persistence.Configurations;

public class TradeConfiguration : IEntityTypeConfiguration<Trade>
{
   public void Configure(EntityTypeBuilder<Trade> builder)
   {
      builder.HasKey(x => x.Id);

      builder.Property(x => x.Notes)
          .HasMaxLength(512);

      // all decimals are configured via ApplicationDbContext.ConfigureConventions()
   }
}
