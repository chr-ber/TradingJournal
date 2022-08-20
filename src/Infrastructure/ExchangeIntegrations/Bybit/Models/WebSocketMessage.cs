namespace TradingJournal.Infrastructure.ExchangeIntegrations.Bybit.Models;

public class WebSocketMessage<T>
{
   public Topic topic { get; set; }

   public IList<T> data { get; set; }
}

public class WebSocketMessage
{
   public Topic topic { get; set; }
}
