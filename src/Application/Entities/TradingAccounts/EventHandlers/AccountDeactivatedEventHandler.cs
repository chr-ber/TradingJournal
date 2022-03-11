using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Application.Common.Models;
using TradingJournal.Domain.Events;
using MediatR;

namespace TradingJournal.Application.Entities.Accounts.EventHandlers;

public class AccountDeactivatedEventHandler : INotificationHandler<DomainEventNotification<TradingAccountDeactivated>>
{
    private readonly IAccountSynchronizationService _accountSynchronizationService;

    public AccountDeactivatedEventHandler(IAccountSynchronizationService accountSynchronizationService)
    {
        _accountSynchronizationService = accountSynchronizationService;
    }

    public async Task Handle(DomainEventNotification<TradingAccountDeactivated> notification, CancellationToken cancellationToken)
    {
        await _accountSynchronizationService.DeactivateAccount(notification.DomainEvent.Account);
    }
}