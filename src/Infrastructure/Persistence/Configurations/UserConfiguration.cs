using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingJournal.Domain.Entities;


namespace TradingJournal.Infrastructure.Server.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.DisplayName)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.PasswordSalt)
            .IsRequired();

        builder.Property(x => x.PasswordHash)
            .IsRequired();
    }
}