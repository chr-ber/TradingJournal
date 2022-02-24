namespace TradingJournal.Domain.Entities;

public class User
{
    public int Id { get; set; }

    public string DisplayName { get; set; }

    public string Email { get; set; }

    public byte[] PasswordHash { get; set; }

    public byte[] PasswordSalt { get; set; }
}
