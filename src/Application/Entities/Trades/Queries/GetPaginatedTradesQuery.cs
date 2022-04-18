using TradingJournal.Application.Common.Extensions;
using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Application.Common.Models;
using TradingJournal.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TradingJournal.Domain.Enums;

namespace TradingJournal.Application.Trades.Queries;

public class GetPaginatedTradesQuery : IRequest<PaginatedList<Trade>>
{
    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public IEnumerable<TradeStatus> IncludedStates { get; set; }

    public bool Hidden { get; set; }
}

public class GetPaginatedTradesQueryHandler : IRequestHandler<GetPaginatedTradesQuery, PaginatedList<Trade>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetPaginatedTradesQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    public async Task<PaginatedList<Trade>> Handle(GetPaginatedTradesQuery request, CancellationToken cancellationToken)
    {
        int userId = await _currentUserService.GetUserId();

        var response = _context.Trades
            .Include(x => x.TradingAccount)
            .Include(x => x.Symbol)
            .Where(x => x.TradingAccount.UserId == userId)
            .Where(x => x.IsHidden == request.Hidden)
            // filter by state if list of states was provided
            .Where(x => request.IncludedStates != null ? request.IncludedStates.Contains(x.Status) : true)
            .OrderByDescending(x => x.OpenedAt);

        return await response.ToPaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
