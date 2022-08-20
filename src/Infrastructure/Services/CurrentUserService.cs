using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace TradingJournal.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
   private readonly IApplicationDbContext _context;
   private readonly IHttpContextAccessor _httpContextAccessor;
   public CurrentUserService(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
   {
      _context = context ?? throw new ArgumentNullException(nameof(context));
      _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
   }

   public async Task<User> GetUser()
   {
      var userId = await GetUserId();
      return await _context.Users.FindAsync(userId);
   }

   public Task<int> GetUserId()
   {
      var nameIdentifier = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
      return Task.FromResult(int.Parse(nameIdentifier));
   }
}
