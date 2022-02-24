using TradingJournal.Application.Common.Models;

namespace TradingJournal.Application.Common.Interfaces;

public interface IAuthenticationRepository
{
    Task<ServiceResponse<int>> Register(UserRegistration registration);
    Task<ServiceResponse<string>> Login(UserLogin login);
    Task<bool> UserExists(string email);
}