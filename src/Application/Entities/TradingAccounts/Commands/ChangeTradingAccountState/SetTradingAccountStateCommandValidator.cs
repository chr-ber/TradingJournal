namespace TradingJournal.Application.Entities.Accounts.Commands.ChangeTradingAccountState;

public class SetTradingAccountStateCommandValidator : AbstractValidator<SetTradingAccountStateCommand>
{
   private readonly IApplicationDbContext _context;
   private readonly ICurrentUserService _currentUserService;
   private readonly IApiUtilityService _byBitUtilityService;

   public SetTradingAccountStateCommandValidator(
       IApplicationDbContext context,
       ICurrentUserService currentUserService,
       IApiUtilityService byBitUtilityService)
   {
      _context = context;
      _currentUserService = currentUserService;
      _byBitUtilityService = byBitUtilityService;

      RuleFor(p => p)
          .MustAsync((x, y) => IsAccontOwnedByUser(x.Id, y)).WithMessage("The reuqestor is not owner of the account.");
   }

   // ensures that per user accounts must be named unique
   public async Task<bool> IsAccontOwnedByUser(int id, CancellationToken cancellationToken = new())
   {
      var userId = await _currentUserService.GetUserId();
      var userIsOwner = await _context.TradingAccounts.AnyAsync(x => x.Id == id && userId == x.UserId);

      return userIsOwner;
   }
}

