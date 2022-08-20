namespace TradingJournal.Application.Entities.Accounts.Commands.ChangeTradingAccountState;

public class SetTradingAccountStateCommand : IRequest
{
   public int Id { get; set; }

   public TradingAccountStatus Status { get; set; }

}

public class SetTradingAccountStateCommandHandler : IRequestHandler<SetTradingAccountStateCommand>
{
   private readonly IApplicationDbContext _context;
   private readonly ICurrentUserService _currentUserService;

   public SetTradingAccountStateCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
   {
      _context = context;
      _currentUserService = currentUserService;
   }

   public async Task<Unit> Handle(SetTradingAccountStateCommand request, CancellationToken cancellationToken)
   {

      var account = await _context.TradingAccounts.FindAsync(request.Id);

      if (account == null)
         throw new NotFoundException($"Could not find trading account with id {request.Id}");

      // if state is already set as requested state
      if (account.Status == request.Status)
         return Unit.Value;

      account.Status = request.Status;

      if (account.Status == TradingAccountStatus.Enabled)
      {
         account.DomainEvents.Add(new TradingAccountActivated(account));
      }
      else if (account.Status == TradingAccountStatus.Disabled)
      {
         account.DomainEvents.Add(new TradingAccountDeactivated(account));
      }

      await _context.SaveChangesAsync(cancellationToken);

      return Unit.Value;
   }
}
