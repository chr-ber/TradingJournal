using TradingJournal.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using TradingJournal.Domain.Entities;
using MediatR;

namespace TradingJournal.Application.Entities.Trades.Commands.HideTrade;

public class BatchSetTradeVisibilityCommand : IRequest
{
    public IEnumerable<int> Ids { get; set; }

    public bool IsHidden { get; set; }
}

public class BatchUpdateTradeVisibilityCommandHandler : IRequestHandler<BatchSetTradeVisibilityCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    public BatchUpdateTradeVisibilityCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(BatchSetTradeVisibilityCommand request, CancellationToken cancellationToken)
    {
        var userid = await _currentUserService.GetUserId();

        await _context.Trades.Where(x => x.TradingAccount.UserId == userid && request.Ids.Contains(x.Id))
                    .ForEachAsync(x => x.IsHidden = request.IsHidden, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
