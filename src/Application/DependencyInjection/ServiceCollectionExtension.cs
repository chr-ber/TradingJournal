using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Application.ClientServices;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using MediatR;

namespace TradingJournal.Application.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServerApplication(this IServiceCollection services)
    {
        // add mediatr and scan assembly to add handlers, preprocessors, and postprocessors implementations to the container
        services.AddMediatR(Assembly.GetExecutingAssembly());

        // add validators by scanning assembly
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }

    public static IServiceCollection AddClientApplication(this IServiceCollection services)
    {
        services.AddScoped<ITradingAccountService,TradingAccountService>();
        services.AddScoped<IUserPreferncesService, UserPreferncesService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<ITradeService, TradeService>();
        services.AddScoped<UserInterfaceService>();

        return services;
    }
}