using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Domain.Entities;

namespace TradingJournal.Infrastructure.Server.Services;

public partial class DomainEventService
{
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
            string nameIdentifier = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(nameIdentifier);
        }
    }
}