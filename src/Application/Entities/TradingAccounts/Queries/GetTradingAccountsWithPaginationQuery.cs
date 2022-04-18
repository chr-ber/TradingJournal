using TradingJournal.Application.Common.Extensions;
using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Application.Common.Models;
using TradingJournal.Domain.Entities;
using MediatR;

namespace TradingJournal.Application.Entities.Accounts.Queries;

public class GetTradingAccountsWithPaginationQuery : IRequest<PaginatedList<TradingAccount>>
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}

public class GetTradingAccountsWithPaginationQueryHandler : IRequestHandler<GetTradingAccountsWithPaginationQuery,PaginatedList<TradingAccount>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetTradingAccountsWithPaginationQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    public async Task<PaginatedList<TradingAccount>> Handle(GetTradingAccountsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        int userId = await _currentUserService.GetUserId();

        return await _context.TradingAccounts
            .Where(x => x.UserId == userId)
            .ToPaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
