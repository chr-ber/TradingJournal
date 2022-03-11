using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace TradingJournal.Infrastructure.Server.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUserService(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<User> GetUser()
    {
        int userId = await GetUserId();
        return await _context.Users.FindAsync(userId);
    }

    public async Task<int> GetUserId()
    {
        string nameIdentifier = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        return int.Parse(nameIdentifier);
    }
}