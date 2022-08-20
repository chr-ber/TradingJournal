namespace TradingJournal.Infrastructure.Services;

public class AccountSynchronizationService : BackgroundService, IAccountSynchronizationService
{
   private readonly IServiceProvider _services;
   private readonly IConfiguration _configuration;
   private readonly ILogger<AccountSynchronizationService> _logger;

   private CancellationToken _stoppingToken;
   private ISender _mediator = null!;

   // if _mediator is null get it from IServiceProvider and assign it to the local variable
   protected ISender Mediator => _mediator ??= _services.CreateScope().ServiceProvider.GetRequiredService<ISender>();

   private Dictionary<ApiWrapper, int> _apiWrapperAccountIdMap = new();

   public AccountSynchronizationService(
       IServiceProvider services,
       IConfiguration configuration,
       ILogger<AccountSynchronizationService> logger)
   {
      _services = services ?? throw new ArgumentNullException(nameof(services));
      _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
   }

   public Task DeactivateAccount(TradingAccount account)
   {
      if (account == null)
         throw new ArgumentNullException(nameof(account));

      var accountIdPair = _apiWrapperAccountIdMap.FirstOrDefault(x => x.Value == account.Id);

      // if account has no active connection
      if (accountIdPair.Key is null)
         return Task.CompletedTask;

      accountIdPair.Key.Dispose();

      _apiWrapperAccountIdMap.Remove(accountIdPair.Key);

      return Task.CompletedTask;
   }

   public async Task ActivateAccount(TradingAccount account)
   {
      if (account == null)
         throw new ArgumentNullException(nameof(account));

      await CreateAPIWrapper(account);
   }

   private Task CreateAPIWrapper(TradingAccount account)
   {
      using (var scope = _services.CreateScope())
      {
         var apiWrapper = scope.ServiceProvider.GetRequiredService<ApiWrapper>();

         apiWrapper.ApiKey = account.APIKey;
         apiWrapper.ApiSecret = account.APISecret;
         apiWrapper.stoppingToken = _stoppingToken;
         apiWrapper.OnReceivedExecution += NewExecutionHandler;

         apiWrapper.CreateWebSocketConnection();

         _apiWrapperAccountIdMap[apiWrapper] = account.Id;
      }
      return Task.CompletedTask;
   }

   // method is called when the service is started
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

   // will be called whenever there is a new execution received via weboscket connection
   private async Task NewExecutionHandler(List<WebSocketExecution> executions, ApiWrapper apiWrapper, bool isClosing, decimal quantity)
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
