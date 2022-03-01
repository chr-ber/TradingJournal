using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Domain.Entities;
using TradingJournal.Domain.Enums;
using MediatR;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace TradingJournal.Application.Trades.Queries;

public class GetTradeCountQuery : IRequest<int>
{

}

public class GetTradeCountQueryHandler : IRequestHandler<GetTradeCountQuery, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetTradeCountQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(GetTradeCountQuery request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserId();

        return await _context.Trades.CountAsync(x => x.TradingAccount.UserId == userId);
    }
}