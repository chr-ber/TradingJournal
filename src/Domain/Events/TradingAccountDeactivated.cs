namespace TradingJournal.Domain.Events;

public class TradingAccountDeactivated : DomainEvent
{
   public TradingAccount Account { get; set; }

   public TradingAccountDeactivated(TradingAccount account)
   {
      Account = account;
   }
}
