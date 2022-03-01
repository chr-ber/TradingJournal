using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingJournal.Application.Common.Interfaces;

namespace TradingJournal.Application.Trades.Queries;

    public class GetAverageTradeReturnQuery : IRequest<decimal>
    {

    }

public class GetAverageTradeReturnQueryHandler : IRequestHandler<GetAverageTradeReturnQuery, decimal>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetAverageTradeReturnQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<decimal> Handle(GetAverageTradeReturnQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = await _currentUserService.GetUserId();

        var averageReturn = _context.Trades
            .Where(x => x.TradingAccount.UserId == currentUserId && x.Status != Domain.Enums.TradeStatus.Open)
            .Average(x => x.NetReturn);

        return averageReturn;
    }
}

