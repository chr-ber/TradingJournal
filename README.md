## TradingJournal

### About

**TradingJournal** is a **Single Page Application** fully written in C#.

The application was developed as part of the final assignment to complete the 6-month educational program [Training as a certified Software Developer C#](https://www.wifiwien.at/kurs/18196x-ausbildung-zum-geprueften-software-developer-c) at [WIFI Vienna](https://www.wifiwien.at/).

### Project Goal

The goal of the application is to make journaling crypto derivative trades as simple and responsive as possible.

This is achieved by allowing the users to add read only API keys from the trading platform [ByBit.com](https://bybit.com) to the application. Once added a background service will create WebSocket connections to the  trading platform. The trades are imported with only a few milliseconds delay and allow immediate journaling and access to detailed statistics without any manual synchronization or delay.

### Architecture

The design of the application is trying to follow the principles provided by the [Clean Architecture Solution Template](https://github.com/jasontaylordev/CleanArchitecture).

### Technologies

* [Blazor Server 6](https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-6.0#blazor-server)
* [Blazor WebAssembly 6](https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-6.0#blazor-webassembly)
* [ASP.NET Core 6](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0)
* [Entity Framework Core 6](https://docs.microsoft.com/en-us/ef/core/)
* [MediatR](https://github.com/jbogard/MediatR)
* [FluentValidation](https://fluentvalidation.net/)
* [MudBlazor](https://mudblazor.com/)

### Live Demo

Link: [https://tradingjournal.chrisberger.dev](https://tradingjournal.chrisberger.dev/login?Username=Trader1@example.com&Password=Trader1@example.com)

The live demo is using the [ByBit TestNet](https://testnet.bybit.com/). Feel free to add api credentials on the accounts page to test live trade import.

Please note that all data of the demo site gets reset daily at 24:00 UTC.

### Screenshots

![Dashboard](https://raw.githubusercontent.com/chr-ber/TradingJournal/master/docs/screenshots/dashboard.png)

![Trade Details](https://raw.githubusercontent.com/chr-ber/TradingJournal/master/docs/screenshots/trade-details.png)

![Accounts](https://raw.githubusercontent.com/chr-ber/TradingJournal/master/docs/screenshots/accounts.png)

### Getting Started

1. Install the latest [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
2. Navigate to `src/Server` and run `dotnet run` to launch the back end (ASP.NET Core Web API)

#### Database Configuration

The application is configured to use an in-memory database by default. This ensures that all users will be able to run the solution without needing to set up additional infrastructure (e.g. SQL Server).

If you would like to use SQL Server, you will need to update **src/Server/appsettings.json** as follows:

```json
  "UseInMemoryDatabase": false,
```

Verify that the **DefaultConnection** connection string within **appsettings.json** points to a valid SQL Server instance. 

When you run the application the database will be automatically created (if necessary) and the latest migrations will be applied.

#### Database Migrations

To use `dotnet-ef` for your migrations please add the following flags to your command (values assume you are executing from **repository root**)

* `--project src/Infrastructure`
* `--startup-project src/Server`
* `--output-dir Persistence/Migrations`

To add a new migration:

 `dotnet ef migrations add "ExampleMigration" --project src\Infrastructure --startup-project src\Server --output-dir Persistence\Migrations`

To update the database:

`dotnet ef database update --project src\Infrastructure --startup-project src\Server`








