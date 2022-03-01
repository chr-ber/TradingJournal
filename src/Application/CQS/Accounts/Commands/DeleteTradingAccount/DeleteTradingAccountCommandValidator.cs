using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TradingJournal.Application.Common.Interfaces;

namespace TradingJournal.Application.Accounts.Commands.DeleteTradingAccount;

public class DeleteTradingAccountCommandValidator : AbstractValidator<DeleteTradingAccountCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteTradingAccountCommandValidator(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;

        RuleFor(p => p.Id)
            .MustAsync((x,y) => BeRecordOwnedByUser(x,y)).WithMessage("Record is owned by different user.");
    }

    public async Task<bool> BeRecordOwnedByUser(int id, CancellationToken cancellationToken = new())
    {
        int userId = await _currentUserService.GetUserId();
        return await _context.TradingAccounts.AnyAsync(x => x.Id == id && userId == x.UserId);
    }
}

