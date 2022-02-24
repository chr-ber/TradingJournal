using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Domain.Entities;
using TradingJournal.Domain.Events;
using MediatR;
using TradingJournal.Domain.Enums;

namespace TradingJournal.Application.Executions.Commands;

public class BatchImportExecutionsCommand : IRequest
{
    public bool IsClosing { get; set; }

    public int? TradeId { get; set; }

    public int AccountId { get; set; }

    public TradeSide Side { get; set; }

    public int SymbolId { get; set; }

    public decimal OrderQuantitiy { get; set; }

    public TradeDirection Direciton { get; set; }

    public List<Execution> Executions { get; set; }

    public class Execution
    {
        public TradeAction Action { get; set; }

        public DateTime ExecutedAt { get; set; }

        public decimal Size { get; set; }

        public decimal Position { get; set; }

        public decimal Price { get; set; }

        public decimal Fee { get; set; }
    }
}

public class BatchImportExecutionsCommandHandler : IRequestHandler<BatchImportExecutionsCommand>
{
    private readonly IApplicationDbContext _context;

    public BatchImportExecutionsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(BatchImportExecutionsCommand request, CancellationToken cancellationToken)
    {
        if (request.Executions == null || request.Executions.Count == 0)
            throw new ArgumentNullException(nameof(request.Executions),"Provided list of executions is null or empty.");

        // find the most recent execution in the db
        //var lastExecution = _context.Executions
        //    .Where(x => x.TradeId == request.TradeId)
        //    .OrderByDescending(x => x.ExecutedAt)
        //    .FirstOrDefault();

        //var trade = _context.Trades.FirstOrDefault(x => x.Id == request.TradeId);

        Trade trade;

        if(request.TradeId == null)
        {
            trade = new Trade()
            {
                SymbolId = request.SymbolId,
                Side = request.Side,
                TradingAccountId = request.AccountId,
            };
            _context.Trades.Add(trade);
            //await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            trade = _context.Trades.FirstOrDefault(x => x.Id == request.TradeId);
        }

        decimal lastPosition = trade?.Position ?? 0;
        decimal preExecutionRemovedSize = trade.Size - trade.Position;

        for (int i = 0; i < request.Executions.Count; i++)
        {
            decimal newPosition = request.IsClosing ? lastPosition - request.Executions[i].Position : lastPosition + request.Executions[i].Position;

            Execution execution = new()
            {
                Action = request.Executions[i].Action,
                ExecutedAt = request.Executions[i].ExecutedAt,
                Size = request.Executions[i].Size,
                Price = request.Executions[i].Price,
                Fee = request.Executions[i].Fee,
                Position = newPosition,
                Trade = trade,
                Value = request.Executions[i].Size * request.Executions[i].Price,
                Direction = request.Direciton,
            };

            if (request.IsClosing)
            {
                execution.Return = (execution.Price - trade.AverageEntryPrice) * execution.Position;
                execution.NetReturn = execution.Return - execution.Fee;

                trade.AverageExitPrice = (preExecutionRemovedSize * trade.AverageExitPrice + execution.Size * execution.Price) / (preExecutionRemovedSize + execution.Size);
                //trade.Position -= request.Executions[i].Size;
            }
            else
            {
                trade.AverageEntryPrice = (trade.Size * trade.AverageEntryPrice + execution.Size * execution.Price) / (trade.Size + execution.Size); 
            }
            _context.Executions.Add(execution);
            lastPosition = newPosition;
        }

        // handling position and size will be done by the order quantity and not by the total of the invdividual exec_quantities
        // this is necessary as some executions have decimal values but the actual position of the order is rounded
        // if the exec_quantity is used there will be a missmatch between the exchange and the database
        if (request.IsClosing)
        {
            trade.Position -= request.OrderQuantitiy;
        }
        else
        {
            trade.Size += request.OrderQuantitiy;
            trade.Position += request.OrderQuantitiy;
            trade.Cost = trade.AverageEntryPrice * trade.Size;
        }

        decimal postExecutionRemovedSize = trade.Size - trade.Position;
        trade.Return = trade.AverageExitPrice * postExecutionRemovedSize - trade.AverageEntryPrice * postExecutionRemovedSize;
        trade.NetReturn = trade.Return; 

        // if no position remaining, close trade
        if (trade.Position == 0)
        {
            trade.Result = trade.Return switch
            {
                < 0 => TradeResult.Loss,
                > 0 => TradeResult.Win,
                _ => TradeResult.Breakeven,
            };
        }

        //entity.DomainEvents.Add(new TradingAccountCreatedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

