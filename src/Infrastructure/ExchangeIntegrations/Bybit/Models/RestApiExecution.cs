namespace TradingJournal.Infrastructure.ExchangeIntegrations.Bybit.Models;

public class RestApiExecution
{
   // some order ids are not standardized guids
   public string order_id { get; set; }

   public string exec_id { get; set; }

   public string symbol { get; set; }

   public string side { get; set; }

   public decimal exec_qty { get; set; }

   public decimal closed_size { get; set; }

   public decimal exec_price { get; set; }

   public decimal exec_fee { get; set; }

   public string order_type { get; set; }

   public string exec_type { get; set; }
}
