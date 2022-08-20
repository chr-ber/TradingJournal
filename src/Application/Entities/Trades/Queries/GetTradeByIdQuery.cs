namespace TradingJournal.Application.Trades.Queries;

public class GetTradeByIdQuery : IRequest<Trade>
{
   public int Id { get; set; }
}

public class GetTradeByIdQueryHandler : IRequestHandler<GetTradeByIdQuery, Trade>
{
   private readonly IApplicationDbContext _context;
   private readonly ICurrentUserService _currentUserService;

   public GetTradeByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
   {
      _context = context;
      _currentUserService = currentUserService;
   }

   public async Task<Trade> Handle(GetTradeByIdQuery request, CancellationToken cancellationToken)
   {
      var userId = await _currentUserService.GetUserId();

      var trade = await _context.Trades
          .Include(x => x.Symbol)
          .Include(x => x.TradingAccount)
          .Include(x => x.Executions)
          .FirstOrDefaultAsync(x => x.Id == request.Id && x.TradingAccount.UserId == userId);

      return trade;
   }
}
