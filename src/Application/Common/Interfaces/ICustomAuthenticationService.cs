using TradingJournal.Application.Common.Models;

namespace TradingJournal.Application.Common.Interfaces;

public interface ICustomAuthenticationService
{
    Task<ServiceResponse<string>> Login(UserLogin request);
    Task Logout();
    Task<ServiceResponse<int>> Register(UserRegistration request);
}
