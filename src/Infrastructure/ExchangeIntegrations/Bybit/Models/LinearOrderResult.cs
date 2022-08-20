namespace TradingJournal.Infrastructure.Server.ExchangeIntegrations.Bybit.Models;

public class LinearOrderResultBase
{
   public int ret_code { get; set; }

   public string ret_msg { get; set; }

   public LinearOrderResult result { get; set; }

   public int rate_limit_status { get; set; }

   public int rate_limit { get; set; }
}

public partial class LinearOrderResult
{
   public string order_id { get; set; }

   public string symbol { get; set; }

   public string side { get; set; }

   public decimal price { get; set; }

   public decimal qty { get; set; }

   public string created_time { get; set; }

   public decimal cum_exec_fee { get; set; }

   public decimal cum_exec_qty { get; set; }

   public decimal cum_exec_value { get; set; }

   public decimal last_exec_price { get; set; }

   public string order_link_id { get; set; }

   public string order_status { get; set; }

   public string order_type { get; set; }

   public bool reduce_only { get; set; }
}
