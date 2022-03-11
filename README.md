TradingJournal is a Single Page Application fully written in C#.

The goal of the app is to make journaling trades executed on the exchange bybit.com effortless.

This is achieved by allowing the users to add read only api keys from the exchange to the application, which will then import all trades by listening to the websocket endpoints for:
* [USDT Perpetual](https://bybit-exchange.github.io/docs/linear/#t-websocket)
* [Inverse Perpetual](https://bybit-exchange.github.io/docs/inverse/#t-websocket)

The design of the application is trying to follow the principles provided by the [Clean Architecture Solution Template](https://github.com/jasontaylordev/CleanArchitecture).

## Technologies

* [Blazor Server 6](https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-6.0#blazor-server)
* [Blazor WebAssembly 6](https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-6.0#blazor-webassembly)
* [ASP.NET Core 6](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0)
* [Entity Framework Core 6](https://docs.microsoft.com/en-us/ef/core/)
* [MediatR](https://github.com/jbogard/MediatR)
* [FluentValidation](https://fluentvalidation.net/)
