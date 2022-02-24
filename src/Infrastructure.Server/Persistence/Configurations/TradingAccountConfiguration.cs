using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingJournal.Domain.Entities;


namespace TradingJournal.Infrastructure.Server.Persistence.Configurations;

public class TradingAccountConfiguration : IEntityTypeConfiguration<TradingAccount>
{
    public void Configure(EntityTypeBuilder<TradingAccount> builder)
    {
        builder.Ignore(e => e.DomainEvents);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.APIKey)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.APISecret)
            .HasMaxLength(256)
            .IsRequired();
    }
}