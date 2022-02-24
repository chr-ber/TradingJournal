using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Domain.Entities;
using TradingJournal.Domain.Events;
using TradingJournal.Domain.Enums;
using MediatR;
using TradingJournal.Application.Common.Exceptions;

namespace TradingJournal.Application.TradingAccounts.Commands.ChangeTradingAccountState;

public class SetTradingAccountStateCommand : IRequest
{
    public int Id { get; set; }

    public TradingAccountStatus Status { get; set; }

}

public class ChangeTradingAccountStateCommandHandler : IRequestHandler<SetTradingAccountStateCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ChangeTradingAccountStateCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(SetTradingAccountStateCommand request, CancellationToken cancellationToken)
    {

        var account = await _context.TradingAccounts.FindAsync(request.Id);

        if (account == null)
            throw new NotFoundException($"Could not find trading account with id {request.Id}");

        // if state is already set as requested state
        if (account.Status == request.Status)
            return Unit.Value;

        account.Status = request.Status;

        //entity.DomainEvents.Add(new TradingAccountCreatedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
