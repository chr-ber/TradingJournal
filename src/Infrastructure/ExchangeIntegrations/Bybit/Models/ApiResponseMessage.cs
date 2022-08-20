namespace TradingJournal.Infrastructure.Server.ExchangeIntegrations.Bybit.Models;

public class ApiResponse<T>
{
   public int ret_code { get; set; }

   public string ret_msg { get; set; }

   public Result<T> result { get; set; }

   public int rate_limit_status { get; set; }

   public int rate_limit { get; set; }
}

public class Result<T>
{
   public int current_page { get; set; }

   public List<T> data { get; set; } = new();
}
