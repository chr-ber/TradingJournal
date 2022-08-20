namespace TradingJournal.Application.Trades.Queries;

public class GetOpenTradeBySymbolIdQuery : IRequest<Trade>
{
   public int SymbolId { get; set; }

   public TradeSide Side { get; set; }
}

public class GetOpenTradeBySymbolIdQueryHandler : IRequestHandler<GetOpenTradeBySymbolIdQuery, Trade>
{
   private readonly IApplicationDbContext _context;

   public GetOpenTradeBySymbolIdQueryHandler(IApplicationDbContext context)
   {
      _context = context;
   }

   public async Task<Trade> Handle(GetOpenTradeBySymbolIdQuery request, CancellationToken cancellationToken)
   {
      var trade = await _context.Trades.Where(
              x => x.Status == TradeStatus.Open &&
              x.SymbolId == request.SymbolId &&
              x.Side == request.Side)
          .FirstOrDefaultAsync();

      return trade;

   }
}
