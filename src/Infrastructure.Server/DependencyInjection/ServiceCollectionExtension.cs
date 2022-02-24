using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Infrastructure.Server.Authentication;
using TradingJournal.Infrastructure.Server.Persistence;
using TradingJournal.Infrastructure.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static TradingJournal.Infrastructure.Server.Services.DomainEventService;
using TradingJournal.Infrastructure.Server.ExchangeIntegrations.Bybit;

namespace TradingJournal.Infrastructure.Server.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServerInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // database configuration
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default"),
            b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        // add the created dbcontext as interface
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        // authentication configuration
        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(configuration.GetSection("JavaWebTokenSettings:EncryptionKey").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

        services.AddScoped<IDomainEventService, DomainEventService>();

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<IUtilityService, UtilityService>();

        services.AddTransient<IByBitApiWrapper, ByBitApiWrapper>();

        //services.AddHostedService<AccountSynchronizationService>();

        return services;
    }
}

