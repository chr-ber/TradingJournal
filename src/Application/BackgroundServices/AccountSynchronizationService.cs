using TradingJournal.Application.Executions.Commands;
using TradingJournal.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using TradingJournal.Application.Common.Interfaces;
using MediatR;
using TradingJournal.Infrastructure.Server.ExchangeIntegrations.Bybit;
using TradingJournal.Application.Common.Models;
using TradingJournal.Application.Trades.Queries;
using TradingJournal.Application.Symbols.Commands;

namespace TradingJournal.Application.BackgroundServices;

public class AccountSynchronizationService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AccountSynchronizationService> _logger;

    private ISender _mediator = null!;
    // if _mediator is null get it from IServiceProvider and assign it to the local variable
    protected ISender Mediator => _mediator ??= _services.CreateScope().ServiceProvider.GetRequiredService<ISender>();

    private Timer _timer;

    private Dictionary<IByBitApiWrapper, int> _apiWrapperAccountIdMap = new();

    public AccountSynchronizationService(
        IServiceProvider services,
        IConfiguration configuration,
        ILogger<AccountSynchronizationService> logger)
    {
        _services = services;
        _configuration = configuration;
        _logger = logger;
    }

    public void StopAccount(int accountId)
    {
        var keyValuePair = _apiWrapperAccountIdMap.FirstOrDefault(x => x.Value == accountId);

        if (keyValuePair.Key is null)
            throw new Exception("Trading account is not running."); // todo: update exception

        keyValuePair.Key.Dispose();
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = _services.CreateScope())
        using (var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>())
        {
            // get all enabled tradingaccounts
            var activeAccounts = context.TradingAccounts
                .Where(x => x.Status == TradingAccountStatus.Enabled)
                .ToList();

            // create an api wrapper for each account and start the websocket connection
            foreach (var account in activeAccounts)
            {
                var apiWrapper = scope.ServiceProvider.GetRequiredService<IByBitApiWrapper>();

                apiWrapper.ApiKey = account.APIKey;
                apiWrapper.ApiSecret = account.APISecret;
                apiWrapper.stoppingToken = stoppingToken;
                apiWrapper.OnReceivedExecution += NewExecutionHandler;

                apiWrapper.CreateWebSocketConnection();

                _apiWrapperAccountIdMap[apiWrapper] = account.Id;

                //WebSocketConnection inverseWs = new(stoppingToken, ContractEndpoint.InversePerpetual)
                //{
                //    APIKey = account.APIKey,
                //    APISecret = account.APISecret,
                //};

                //inverseWs.Connect();
                //inverseWs.ExecutionEventOccured += ((x, y) =>
                //{
                //    _mediator.Send(new CreateExecutionCommand()
                //    {
                //        Account = account,
                //        OrderId = y.order_id,
                //        Contract = y.contract,
                //    });
                //});

                //WebSocketConnection usdtWs = new(stoppingToken, ContractEndpoint.USDTPerpetual)
                //{
                //    APIKey = account.APIKey,
                //    APISecret = account.APISecret,
                //};

                //usdtWs.Connect();
                //usdtWs.ExecutionEventOccured += ((x, y) =>
                //{
                //    _mediator.Send(new CreateExecutionCommand()
                //    {
                //        Account = account,
                //        OrderId = y.order_id,
                //        Contract = y.contract,
                //    });
                //});

                //_connections.Add(inverseWs);
                //_connections.Add(usdtWs);
            }
        }

        // reoccuring job to handle 
        //_timer = new(ValidateWebsocketConnection, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
    }

    //public void ValidateWebsocketConnection(object state)
    //{
    //    Debug.WriteLine($"\r\n{DateTime.Now} - Checking if any websocket connection disconnected...");
    //    using (var scope = _services.CreateScope())
    //    using (var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>())
    //    {
    //        foreach (var connection in _connections)
    //        {
    //            if (connection.IsRunning is false)
    //            {
    //                Debug.WriteLine($"\r\n{DateTime.Now} - {connection.APIKey} has stopped running...");
    //                // TODO: test if the api key is still functioning (not expired or deleted from the exchange
    //                //       if api key is ok, restart the websocket connection
    //            }
    //        }
    //    }
    //}

    private async Task NewExecutionHandler(List<WebSocketExecution> executions, IByBitApiWrapper apiWrapper, bool isClosing, decimal quantity)
    {

        var execution = executions.FirstOrDefault();

        // if trade is NOT closing and sell is short
        var tradeSide = isClosing is false && execution.side.ToLower() == "sell" ? TradeSide.Short : TradeSide.Long;

        var symbolId = await Mediator.Send(new CreateSymbolIfMissingCommand()
        {
            Name = execution.symbol,
        });

        var trade = await Mediator.Send(new GetOpenTradeBySymbolIdQuery()
        {
            Side = tradeSide,
            SymbolId = symbolId,
        });

        var totalFee = executions.Sum(x => x.exec_fee);
        var totalQuanitity = execution.order_qty;
        var averagePrice = executions.Average(x => x.price);
        var totalCost = averagePrice * execution.order_qty;

        int? tradeId = null;

        if(trade != null)
        {
            tradeId = trade.Id;
        }

        // get trade direction

        var tradeDirection = isClosing switch
        {
            true => TradeDirection.Close,
            false => TradeDirection.Open,
        };

        //// if not trade was found, create one
        //if (trade == null)
        //{
        //    tradeId = await Mediator.Send(new CreateTradeCommand()
        //    {
        //        AccountId = _apiWrapperAccountIdMap[apiWrapper],
        //        SymbolId = symbolId,
        //        Side = tradeSide,
        //        Size = totalQuanitity,
        //        Position = totalQuanitity,
        //        Cost = totalCost,
        //        AveragePrice = averagePrice,
        //        NetCost = totalFee,
        //    });
        //}
        //else
        //{
        //    tradeId = trade.Id;
        //}

        var command = new BatchImportExecutionsCommand()
        {
            TradeId = tradeId,
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


        //Mediator.Send(new CreateExecutionCommand()
        //{
        //    Account = account,
        //    OrderId = y.order_id,
        //    Contract = y.symbol,
        //});
    }
}
