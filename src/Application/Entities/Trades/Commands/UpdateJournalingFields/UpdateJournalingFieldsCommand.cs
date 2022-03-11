using MediatR;
using Microsoft.EntityFrameworkCore;
using TradingJournal.Application.Common.Interfaces;

namespace TradingJournal.Application.Entities.Trades.Commands.UpdateJournalingFields;
public class UpdateJournalingFieldsCommand : IRequest
{
    public string Notes { get; set; }

    public int Confluences { get; set; }

    public int TradeId { get; set; }
}

public class UpdateJournalingFieldsCommandHandler : IRequestHandler<UpdateJournalingFieldsCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateJournalingFieldsCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(UpdateJournalingFieldsCommand request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserId();
        var trade = await _context.Trades.FirstOrDefaultAsync(x => x.Id == request.TradeId && x.TradingAccount.UserId == userId);

        trade.Notes = request.Notes;
        trade.Confluences = request.Confluences;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
