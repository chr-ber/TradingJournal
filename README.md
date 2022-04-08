TradingJournal is a Single Page Application fully written in C#.

## Overview

### About

The application was developed as the final project for my [Training as a certified Software Developer C#](https://www.wifiwien.at/kurs/18196x-ausbildung-zum-geprueften-software-developer-c)

### Project Goal

The goal of the application is to make journaling crypto derivative trades as simple and responsive as possible.

This is achieved by allowing the users to add read only api keys from the trading platform [ByBit.com](https://bybit.com) to the application. Once added a background service will create websocket connections and import trades with only a few milliseconds delay.

ByBit Websocket Endpooints:
* [USDT Perpetual](https://bybit-exchange.github.io/docs/linear/#t-websocket)
* [Inverse Perpetual](https://bybit-exchange.github.io/docs/inverse/#t-websocket)

### Architecture

The design of the application is trying to follow the principles provided by the [Clean Architecture Solution Template](https://github.com/jasontaylordev/CleanArchitecture).

### Technologies

* [Blazor Server 6](https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-6.0#blazor-server)
* [Blazor WebAssembly 6](https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-6.0#blazor-webassembly)
* [ASP.NET Core 6](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0)
* [Entity Framework Core 6](https://docs.microsoft.com/en-us/ef/core/)
* [MediatR](https://github.com/jbogard/MediatR)
* [FluentValidation](https://fluentvalidation.net/)









