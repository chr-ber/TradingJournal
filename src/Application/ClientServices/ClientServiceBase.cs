namespace TradingJournal.Application.ClientServices;

// base class for all client services
public class ClientServiceBase
{
   protected readonly JsonSerializerOptions _jsonOptions;

   public ClientServiceBase()
   {
      // ignore case sensivity
      // required as api controllers serialize properties to lower case
      // but when desierializing they are by default expected to match pascal case naming convention
      _jsonOptions = new()
      {
         PropertyNameCaseInsensitive = true
      };
   }
}

