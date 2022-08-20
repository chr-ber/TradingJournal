namespace TradingJournal.Infrastructure.Server.ExchangeIntegrations.Bybit.Models;

public class WebSocketExecution
{
   public string symbol { get; set; }

   public string side { get; set; }

   public string order_id { get; set; }

   public string exec_id { get; set; }

   public string order_link_id { get; set; }

   public decimal price { get; set; }

   public decimal order_qty { get; set; }

   public string exec_type { get; set; }

   public decimal exec_qty { get; set; }

   public decimal exec_fee { get; set; }

   public decimal leaves_qty { get; set; }

   public bool is_maker { get; set; }

   public DateTime trade_time { get; set; }

}
