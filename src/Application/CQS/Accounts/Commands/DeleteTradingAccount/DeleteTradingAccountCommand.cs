using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Domain.Entities;
using TradingJournal.Domain.Events;
using MediatR;
using TradingJournal.Application.Common.Exceptions;

namespace TradingJournal.Application.Accounts.Commands.DeleteTradingAccount;

public class DeleteTradingAccountCommand : IRequest
{
    public int Id { get; set; }
}

public partial class DeleteTradingAccountCommandCommandHandler : IRequestHandler<DeleteTradingAccountCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteTradingAccountCommandCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(DeleteTradingAccountCommand request, CancellationToken cancellationToken)
    {
        User user = await _currentUserService.GetUser();

        var entity = await _context.TradingAccounts
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity is null)
        {
            throw new NotFoundException(nameof(TradingAccount), request.Id);
        }

        // TODO: implement proper event
        //entity.DomainEvents.Add(new TradingAccountDeletedEvent(entity));

        _context.TradingAccounts.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
