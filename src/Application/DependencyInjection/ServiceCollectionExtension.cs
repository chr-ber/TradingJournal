using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;
using FluentValidation;
using TradingJournal.Application.Common.Interfaces;
using Blazored.Toast;
using TradingJournal.Application.BackgroundServices;
using TradingJournal.Application.ClientServices;

namespace TradingJournal.Application.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServerApplication(this IServiceCollection services)
    {
        // add mediatr and scan assembly to add handlers, preprocessors, and postprocessors implementations to the container
        services.AddMediatR(Assembly.GetExecutingAssembly());

        // add validators by scanning assembly
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // add object-object mapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddHostedService<AccountSynchronizationService>();

        return services;
    }

    public static IServiceCollection AddClientApplication(this IServiceCollection services)
    {
        services.AddScoped<ITradingAccountService,AccountService>();
        services.AddScoped<ITradeService,TradeService>();
        services.AddScoped<IUserPreferncesService, UserPreferncesService>();
        services.AddScoped<UserInterfaceService>();
        services.AddScoped<IReportService, ReportService>();

        services.AddBlazoredToast();

        return services;
    }
}