namespace TradingJournal.Application.Entities.Accounts.EventHandlers;

public class AccountActivatedEventHandler : INotificationHandler<DomainEventNotification<TradingAccountActivated>>
{
   private readonly IAccountSynchronizationService _accountSynchronizationService;

   public AccountActivatedEventHandler(IAccountSynchronizationService accountSynchronizationService)
   {
      _accountSynchronizationService = accountSynchronizationService;
   }

   public async Task Handle(DomainEventNotification<TradingAccountActivated> notification, CancellationToken cancellationToken)
   {
      await _accountSynchronizationService.ActivateAccount(notification.DomainEvent.Account);
   }
}
