using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Domain.Entities;
using TradingJournal.Domain.Events;
using MediatR;

namespace TradingJournal.Application.TradingAccounts.Commands.CreateTradingAccount;

public class CreateTradingAccountCommand : IRequest<int>
{
    public string Name { get; set; }

    public string APIKey { get; set; }

    public string APISecret { get; set; }

}

public class CreateTradingAccountCommandHandler : IRequestHandler<CreateTradingAccountCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateTradingAccountCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(CreateTradingAccountCommand request, CancellationToken cancellationToken)
    {
        User user = await _currentUserService.GetUser();

        TradingAccount entity = new()
        {
            Name = request.Name,
            APIKey = request.APIKey,
            APISecret = request.APISecret,
            User = user,
        };

        entity.DomainEvents.Add(new TradingAccountCreatedEvent(entity));

        _context.TradingAccounts.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
