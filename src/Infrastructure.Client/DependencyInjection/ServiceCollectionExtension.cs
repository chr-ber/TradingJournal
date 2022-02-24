using Blazored.LocalStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using TradingJournal.Infrastructure.Client.Authentication;
using TradingJournal.Application.Common.Interfaces;


namespace TradingJournal.Infrastructure.Client.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddClientInfrastructure(this IServiceCollection services, IConfiguration Configuration)
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
