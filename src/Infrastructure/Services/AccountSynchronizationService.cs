﻿using TradingJournal.Application.Entities.Executions.Commands.BatchImportExecutions;
using TradingJournal.Application.Entities.Symbols.Commands.CreateSymbolIfMissing;
using TradingJournal.Infrastructure.Server.ExchangeIntegrations.Bybit.Models;
using TradingJournal.Infrastructure.Server.ExchangeIntegrations.Bybit;
using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Application.Trades.Queries;
using TradingJournal.Domain.Entities;
using TradingJournal.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace TradingJournal.Infrastructure.Server.Services;

public class AccountSynchronizationService : BackgroundService, IAccountSynchronizationService
{
    private readonly IServiceProvider _services;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AccountSynchronizationService> _logger;

    private CancellationToken _stoppingToken;
    private ISender _mediator = null!;
    // if _mediator is null get it from IServiceProvider and assign it to the local variable
    protected ISender Mediator => _mediator ??= _services.CreateScope().ServiceProvider.GetRequiredService<ISender>();

    private Dictionary<ByBitApiWrapper, int> _apiWrapperAccountIdMap = new();

    public AccountSynchronizationService(
        IServiceProvider services,
        IConfiguration configuration,
        ILogger<AccountSynchronizationService> logger)
    {
        _services = services;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task DeactivateAccount(TradingAccount account)
    {
        if (account == null)
            throw new ArgumentNullException(nameof(account));

        var accountIdPair = _apiWrapperAccountIdMap.FirstOrDefault(x => x.Value == account.Id);

        // if account has no active connection
        if (accountIdPair.Key is null)
            return;

        accountIdPair.Key.Dispose();

        _apiWrapperAccountIdMap.Remove(accountIdPair.Key);
    }

    public async Task ActivateAccount(TradingAccount account)
    {
        if (account == null)
            throw new ArgumentNullException(nameof(account));

        await CreateAPIWrapper(account);
    }

    private async Task CreateAPIWrapper(TradingAccount account)
    {
        using (var scope = _services.CreateScope())
        {
            var apiWrapper = scope.ServiceProvider.GetRequiredService<ByBitApiWrapper>();

            apiWrapper.ApiKey = account.APIKey;
            apiWrapper.ApiSecret = account.APISecret;
            apiWrapper.stoppingToken = _stoppingToken;
            apiWrapper.OnReceivedExecution += NewExecutionHandler;

            apiWrapper.CreateWebSocketConnection();

            _apiWrapperAccountIdMap[apiWrapper] = account.Id;
        }
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _stoppingToken = stoppingToken;

        using (var scope = _services.CreateScope())
        using (var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>())
        {
            // get all enabled tradingaccounts
            var activeAccounts = await context.TradingAccounts
                .Where(x => x.Status == TradingAccountStatus.Enabled)
                .ToListAsync();

            // create an api wrapper for each account and start the websocket connection
            foreach (var account in activeAccounts)
            {
                await CreateAPIWrapper(account);
            }
        }
    }

    private async Task NewExecutionHandler(List<WebSocketExecution> executions, ByBitApiWrapper apiWrapper, bool isClosing, decimal quantity)
    {
        // get any of the executions in the list
        var execution = executions.FirstOrDefault();

        // get trade side
        var closingShort = isClosing && execution.side.ToLower() == "buy";
        var openingShort = isClosing is false && execution.side.ToLower() == "sell";
        var tradeSide = closingShort || openingShort ? TradeSide.Short : TradeSide.Long;

        // get symbolid, create if missing
        var symbolId = await Mediator.Send(new CreateSymbolIfMissingCommand()
        {
            Name = execution.symbol,
        });

        // search for an open trade
        var trade = await Mediator.Send(new GetOpenTradeBySymbolIdQuery()
        {
            Side = tradeSide,
            SymbolId = symbolId,
        });

        var totalFee = executions.Sum(x => x.exec_fee);
        var totalQuanitity = execution.order_qty;
        var averagePrice = executions.Average(x => x.price);
        var totalCost = averagePrice * execution.order_qty;

        // get trade direction
        var tradeDirection = isClosing ? TradeDirection.Close : TradeDirection.Open;

        var command = new BatchImportExecutionsCommand()
        {
            TradeId = trade != null ? trade.Id : null,
            IsClosing = isClosing,
            Direciton = tradeDirection,
            SymbolId = symbolId,
            Side = tradeSide,
            OrderQuantitiy = quantity,
            AccountId = _apiWrapperAccountIdMap[apiWrapper],
            Executions = new(),
        };

        foreach (var exec in executions)
        {
            command.Executions.Add(new()
            {
                Action = TradeAction.Buy,
                ExecutedAt = exec.trade_time,
                Fee = exec.exec_fee,
                Position = exec.exec_qty,
                Price = exec.price,
                Size = exec.exec_qty,
            });
        }

        await Mediator.Send(command);
    }
}