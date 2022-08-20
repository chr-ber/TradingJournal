namespace TradingJournal.Infrastructure.Server.ExchangeIntegrations.Bybit.Models;

public class ApiKeyInfo
{
   public bool read_only { get; set; }

   public DateTime expired_ad { get; set; }

   public DateTime created_at { get; set; }
}
