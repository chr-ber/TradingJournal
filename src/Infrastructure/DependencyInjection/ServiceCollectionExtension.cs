namespace TradingJournal.Infrastructure.Server.DependencyInjection;

public static class ServiceCollectionExtension
{
   public static IServiceCollection AddServerInfrastructure(this IServiceCollection services, IConfiguration configuration)
   {
      if (configuration.GetValue<bool>("UseInMemoryDatabase"))
      {
         services.AddDbContext<ApplicationDbContext>(options =>
             options.UseInMemoryDatabase("TradingJournal"));
      }
      else
      {
         // database configuration
         services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(configuration.GetConnectionString("Default"),
                 b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
      }

      // add the created dbcontext as interface
      services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

      services.AddScoped<IDomainEventService, DomainEventService>();

      services.AddScoped<ICurrentUserService, CurrentUserService>();

      services.AddScoped<IApiUtilityService, ApiUtilityService>();

      services.AddTransient<ApiWrapper>();

      // create your service as hosted (runs and stops at host's start and shutdown), as well as gets injected as depedency wherever you require it to be
      services.AddSingleton<IAccountSynchronizationService, AccountSynchronizationService>();
      services.AddHostedService(p => p.GetRequiredService<IAccountSynchronizationService>() as AccountSynchronizationService);

      return services;
   }
}

