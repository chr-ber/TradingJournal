using TradingJournal.Client.DependencyInjection;
using TradingJournal.Application.DependencyInjection;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Web;
using TradingJournal.Client;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// add authentication
builder.Services.AddCustomAuthentication(builder.Configuration);
// add services from application library
builder.Services.AddClientApplication();
// add ui package
builder.Services.AddMudServices();

await builder.Build().RunAsync();