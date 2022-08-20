namespace TradingJournal.Infrastructure.Server.Services;

public partial class DomainEventService : IDomainEventService
{
   private readonly ILogger<DomainEventService> _logger;

   private readonly IPublisher _mediator;
   public DomainEventService(ILogger<DomainEventService> logger, IPublisher mediator)
   {
      _logger = logger;
      _mediator = mediator;
   }
   public async Task Publish(DomainEvent domainEvent)
   {
      _logger.LogInformation($"Publishing domain event. Event - {domainEvent.GetType().Name}");
      await _mediator.Publish(GetNotificationForDomainEvent(domainEvent));
   }

   private INotification GetNotificationForDomainEvent(DomainEvent domainEvent)
   {
      var eventType = typeof(DomainEventNotification<>)
          .MakeGenericType(domainEvent.GetType());

      return (INotification)Activator.CreateInstance(eventType, domainEvent);
   }
}
