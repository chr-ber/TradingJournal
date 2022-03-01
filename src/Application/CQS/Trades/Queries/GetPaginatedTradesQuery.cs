using TradingJournal.Application.Common.Extensions;
using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Application.Common.Models;
using TradingJournal.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TradingJournal.Domain.Enums;

namespace TradingJournal.Application.Trades.Queries;

public class GetPaginatedTradesQuery : IRequest<PaginatedList<Trade>>
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public IEnumerable<TradeStatus> IncludedStates { get; set; }
}

public class GetPaginatedTradesQueryHandler
    : IRequestHandler<GetPaginatedTradesQuery, PaginatedList<Trade>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _userUtilityService;

    public GetPaginatedTradesQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ICurrentUserService userUtilityService)
    {
        _context = context;
        _mapper = mapper;
        _userUtilityService = userUtilityService;
    }
    public async Task<PaginatedList<Trade>> Handle(GetPaginatedTradesQuery request, CancellationToken cancellationToken)
    {
        int userId = await _userUtilityService.GetUserId();

        var response = _context.Trades
            .Include(x => x.TradingAccount)
            .Include(x => x.Symbol)
            .Where(x => x.TradingAccount.UserId == userId)
            .Where(x => request.IncludedStates != null ? request.IncludedStates.Contains(x.Status) : true)
            .OrderByDescending(x => x.OpenedAt);

        //if (request.InlucdedStates != null)
        //    //response.Where(x => request.InlucdedStates.Contains(x.Status));
        //    response.Where(x => x.Status == TradeStatus.Win);

        return await response.PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
