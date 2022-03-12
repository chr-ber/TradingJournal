using Microsoft.AspNetCore.Authentication.JwtBearer;
using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Server.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TradingJournal.Server.DependencyInjection;

public static class ServiceCollectionExtension
{
    // System.Security.Cryptography is not allowed within a WASM project
    // therefore all authentication related classes have been added to the client or server directly
    // more details at https://docs.microsoft.com/en-us/dotnet/core/compatibility/cryptography/5.0/cryptography-apis-not-supported-on-blazor-webassembly
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
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

        return services;
    }
}