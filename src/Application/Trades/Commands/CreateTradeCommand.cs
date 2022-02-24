using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Domain.Entities;
using TradingJournal.Domain.Enums;

namespace TradingJournal.Application.Trades.Commands;

public class CreateTradeCommand : IRequest<int>
{

    public int AccountId { get; set; }

    public TradeSide Side { get; set; }

    public int SymbolId { get; set; }

    public decimal Size { get; set; }

    public decimal Position { get; set; }

    public decimal AveragePrice { get; set; }

    public decimal Cost { get; set; }

    public decimal NetCost { get; set; }
}

public class CreateTradeCommandHandler : IRequestHandler<CreateTradeCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateTradeCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateTradeCommand request, CancellationToken cancellationToken)
    {
        Trade entity = new()
        {
            TradingAccountId = request.AccountId,
            Side = request.Side,
            SymbolId = request.SymbolId,
            Size = request.Position,
            Position = request.Position,
            Cost = request.Cost,
            AverageEntryPrice = request.AveragePrice,
            AverageExitPrice = 0,
            Return = 0,
            ReturnPercentage = 0,
            NetReturn = request.NetCost,
            Result = TradeResult.Open,
        };

        //entity.DomainEvents.Add(new TradingAccountCreatedEvent(entity));

        _context.Trades.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
