namespace TradingJournal.Application.Entities.Accounts.Commands.DeleteTradingAccount;

public class DeleteTradingAccountCommand : IRequest
{
   public int Id { get; set; }
}

public partial class DeleteTradingAccountCommandCommandHandler : IRequestHandler<DeleteTradingAccountCommand>
{
   private readonly IApplicationDbContext _context;
   private readonly ICurrentUserService _currentUserService;

   public DeleteTradingAccountCommandCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
   {
      _context = context;
      _currentUserService = currentUserService;
   }

   public async Task<Unit> Handle(DeleteTradingAccountCommand request, CancellationToken cancellationToken)
   {
      var user = await _currentUserService.GetUser();

      var account = await _context.TradingAccounts
          .FindAsync(new object[] { request.Id }, cancellationToken);

      if (account is null)
      {
         throw new NotFoundException(nameof(TradingAccount), request.Id);
      }

      account.DomainEvents.Add(new TradingAccountDeactivated(account));

      _context.TradingAccounts.Remove(account);

      await _context.SaveChangesAsync(cancellationToken);

      return Unit.Value;
   }
}
