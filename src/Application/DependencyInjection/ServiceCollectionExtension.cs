namespace TradingJournal.Application.DependencyInjection;

public static class ServiceCollectionExtension
{
   // extension for loading server dependencies
   public static IServiceCollection AddServerApplication(this IServiceCollection services, IConfiguration configuration)
   {
      // add mediatr and scan assembly to add handlers, preprocessors, and postprocessors implementations to the container
      services.AddMediatR(Assembly.GetExecutingAssembly());

      // add validator implementations by scanning assembly
      services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

      return services;
   }

   // extension for loading client dependencies
   public static IServiceCollection AddClientApplication(this IServiceCollection services)
   {
      services.AddScoped<ITradingAccountService, TradingAccountService>();
      services.AddScoped<IUserPreferncesService, UserPreferncesService>();
      services.AddScoped<IReportService, ReportService>();
      services.AddScoped<ITradeService, TradeService>();
      services.AddScoped<UserInterfaceService>();

      return services;
   }
}
