using Microsoft.AspNetCore.Components.Authorization;
using TradingJournal.Client.Authentication;
using TradingJournal.Application.Common.Interfaces;
using Blazored.LocalStorage;

namespace TradingJournal.Client.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration Configuration)
    {
        // authentication configuration
        services.AddScoped<ICustomAuthenticationService, CustomAuthenticationService>();
        services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
        services.AddAuthorizationCore();
        // mainly used to store jwt on users client
        services.AddBlazoredLocalStorage();

        return services;
    }
}
