using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Domain.Entities;
using TradingJournal.Domain.Enums;
using MediatR;

namespace TradingJournal.Application.Entities.Executions.Commands.CreateExecution;

public class CreateExecutionCommand : IRequest<int>
{
    public int TradeId { get; set; }

    public TradeAction Action { get; set; }

    public DateTime ExecutedAt { get; set; }

    public decimal Size { get; set; }

    public decimal Position { get; set; }

    public decimal Price { get; set; }

    public decimal Fee { get; set; }

    public bool IsClosing { get; set; }

}

public class CreateExecutionCommandHandler : IRequestHandler<CreateExecutionCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;


    public CreateExecutionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(CreateExecutionCommand request, CancellationToken cancellationToken)
    {
        var lastExecution = _context.Executions.Where(x => x.TradeId == request.TradeId)
            .OrderByDescending(x => x.ExecutedAt)
            .FirstOrDefault();

        var position = request.Position;

        if(lastExecution != null)
        {
            position = request.IsClosing ? lastExecution.Position - request.Position : lastExecution.Position + request.Position;
        }

        Execution entity = new()
        {
            Action = request.Action,
            ExecutedAt = request.ExecutedAt,
            Size = request.Size,
            Price = request.Price,
            Fee = request.Fee,
            Position = position,
            TradeId = request.TradeId,
            Value = request.Size * request.Price,
        };

        //entity.DomainEvents.Add(new TradingAccountCreatedEvent(entity));

        _context.Executions.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}

