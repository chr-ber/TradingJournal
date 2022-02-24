using TradingJournal.Application.Common.Extensions;
using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Application.Common.Models;
using TradingJournal.Domain.Entities;
using AutoMapper;
using MediatR;

namespace TradingJournal.Application.TradingAccounts.Queries;

public class GetTradingAccountsWithPaginationQuery : IRequest<PaginatedList<TradingAccount>>
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}

public class GetTradingAccountsWithPaginationQueryHandler
    : IRequestHandler<GetTradingAccountsWithPaginationQuery,PaginatedList<TradingAccount>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _userUtilityService;

    public GetTradingAccountsWithPaginationQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ICurrentUserService userUtilityService)
    {
        _context = context;
        _mapper = mapper;
        _userUtilityService = userUtilityService;
    }
    public async Task<PaginatedList<TradingAccount>> Handle(GetTradingAccountsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        int userId = await _userUtilityService.GetUserId();

        return await _context.TradingAccounts
            .Where(x => x.UserId == userId)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
