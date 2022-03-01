using TradingJournal.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace TradingJournal.Application.Accounts.Commands.ChangeTradingAccountState;

public class ChangeTradingAccountStateCommandValidator : AbstractValidator<SetTradingAccountStateCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUtilityService _byBitUtilityService;

    public ChangeTradingAccountStateCommandValidator(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IUtilityService byBitUtilityService)
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
        int userId = await _currentUserService.GetUserId();
        bool userIsOwner = await _context.TradingAccounts.AnyAsync(x => x.Id == id && userId == x.UserId);

        return userIsOwner;
    }
}

