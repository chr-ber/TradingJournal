using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Domain.Entities;
using TradingJournal.Domain.Enums;
using MediatR;
using System.Diagnostics;

namespace TradingJournal.Application.Trades.Queries;

public class GetOpenTradeBySymbolIdQuery : IRequest<Trade>
{
    public int SymbolId { get; set; }

    public TradeSide Side { get; set; }
}

public class GetTodosQueryHandler : IRequestHandler<GetOpenTradeBySymbolIdQuery, Trade>
{
    private readonly IApplicationDbContext _context;

    public GetTodosQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Trade> Handle(GetOpenTradeBySymbolIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var trade = _context.Trades.Where(
                    x => x.Result == TradeResult.Open &&
                    x.SymbolId == request.SymbolId &&
                    x.Side == request.Side)
                .FirstOrDefault();

            return trade;

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null;
        }
    }
}