using TradingJournal.Infrastructure.Server.DependencyInjection;
using TradingJournal.Application.DependencyInjection;
using TradingJournal.Server.DependencyInjection;
using FluentValidation.AspNetCore;

using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// add json options to controller to ignore circular references when serializing
builder.Services.AddControllersWithViews().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();

// fluent validation added to to server startup instead of application project due to build errors for client project
builder.Services.AddFluentValidation();
// Add dependencies from Infrastructure library
builder.Services.AddServerInfrastructure(builder.Configuration);
// Add dependencies from Application library
builder.Services.AddServerApplication(builder.Configuration);
// Add custom authentication
builder.Services.AddCustomAuthentication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

// enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// apply pending migrations and seed database if enabled
await ApplicationDbContextSeed.SeedAndMigrateDatabase(
    app.Services,
    builder.Configuration.GetValue<bool>("SeedDatabaseWithSampleData"));

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();