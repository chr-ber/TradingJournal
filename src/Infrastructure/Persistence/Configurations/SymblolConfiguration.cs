namespace TradingJournal.Infrastructure.Server.Persistence.Configurations;

public class SymblolConfiguration : IEntityTypeConfiguration<Symbol>
{
   public void Configure(EntityTypeBuilder<Symbol> builder)
   {
      builder.HasKey(x => x.Id);

      builder.Property(x => x.Name)
          .HasMaxLength(16)
          .IsRequired();
   }
}
