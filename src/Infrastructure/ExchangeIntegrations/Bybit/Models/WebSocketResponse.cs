namespace TradingJournal.Infrastructure.Server.ExchangeIntegrations.Bybit.Models;

public class WebSocketResponse<T>
{
   public int ret_code { get; set; }

   public string ret_msg { get; set; }

   public T result { get; set; }
}
