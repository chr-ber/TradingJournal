namespace TradingJournal.Application.Entities.Accounts.Commands.CreateTradingAccount;

public class CreateTradingAccountCommandValidator : AbstractValidator<CreateTradingAccountCommand>
{
   private readonly IApplicationDbContext _context;
   private readonly ICurrentUserService _currentUserService;
   private readonly IApiUtilityService _byBitUtilityService;

   public CreateTradingAccountCommandValidator(
       IApplicationDbContext context,
       ICurrentUserService currentUserService,
       IApiUtilityService byBitUtilityService)
   {
      _context = context;
      _currentUserService = currentUserService;
      _byBitUtilityService = byBitUtilityService;

      RuleFor(p => p.Name)
          .NotEmpty().WithMessage("Name is required.")
          .MaximumLength(128).WithMessage("Name must not exceed 32 characters.");

      RuleFor(p => p.Name)
          .MustAsync((x, y) => BeUniqueNameForUser(x, y)).WithMessage("The name is already in use.");

      RuleFor(p => p.APIKey)
          .MustAsync((x, y) => BeUniqueAPIKeyForUser(x, y)).WithMessage("The key is already in use.");

      RuleFor(p => p.APIKey)
          .NotEmpty().WithMessage("API Key is required.")
          .MaximumLength(256).WithMessage("API Key must not exceed 256 characters.");

      RuleFor(p => p.APISecret)
          .NotEmpty().WithMessage("API Secret is required.")
          .MaximumLength(256).WithMessage("API Secret must not exceed 256 characters.");

      RuleFor(p => p)
          .MustAsync((x, y) => IsReadOnlyApiKey(x.APIKey, x.APISecret, y)).WithMessage("Only api key with read-only permissions allowed. Please create new api key and set it to read-only.");
   }

   // ensures that per user accounts must be named unique
   public async Task<bool> BeUniqueNameForUser(string name, CancellationToken cancellationToken = new())
   {
      var userId = await _currentUserService.GetUserId();
      var nameTaken = await _context.TradingAccounts.AnyAsync(x => x.Name == name && userId == x.UserId);

      return nameTaken is false;
   }

   // ensures that per user an api key must be unique (same trading account not added twice)
   public async Task<bool> BeUniqueAPIKeyForUser(string apiKey, CancellationToken cancellationToken = new())
   {
      var userId = await _currentUserService.GetUserId();
      return await _context.TradingAccounts.AnyAsync(x => x.APIKey == apiKey && userId == x.UserId) is false;
   }

   public async Task<bool> IsReadOnlyApiKey(string apiKey, string apiSecret, CancellationToken cancellationToken)
   {
      return await _byBitUtilityService.IsReadOnlyAPICredentials(apiKey, apiSecret);
   }
}
