namespace TradingJournal.Domain.Events;

public class TradingAccountActivated : DomainEvent
{
   public TradingAccount Account { get; set; }

   public TradingAccountActivated(TradingAccount account)
   {
      Account = account;
   }
}
