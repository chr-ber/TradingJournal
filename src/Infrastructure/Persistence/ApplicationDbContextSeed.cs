namespace Microsoft.AspNetCore.Builder;

public class ApplicationDbContextSeed
{
   public static async Task SeedAndMigrateDatabase(IServiceProvider serviceProvider, bool seedSampleData)
   {
      using var scope = serviceProvider.CreateScope();
      try
      {
         var services = scope.ServiceProvider;
         var context = services.GetRequiredService<ApplicationDbContext>();

         if (context.Database.IsSqlServer())
         {
            // apply pending migrations, creates database if it does not exist
            context.Database.Migrate();
         }

         // seed sample data if enabled in appsettings.json
         if (seedSampleData)
         {
            await SeedSampleData(context);
         }
      }
      catch (Exception ex)
      {
         var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContextSeed>>();
         logger.LogError(ex, "An error occurred while migrating or seeding the database.");
         throw;
      }
   }

   private static async Task SeedSampleData(ApplicationDbContext context)
   {
      // only add sample data when user table is still empty
      if (!context.Users.Any())
      {
         await SeedUsers(context);
         await SeedTradingAccounts(context);
         await SeedSymbols(context);
         await SeedTradesAndExecutions(context);

         await context.SaveChangesAsync();
      }
   }

   private static async Task SeedTradesAndExecutions(ApplicationDbContext context)
   {
      var accounts = await context.TradingAccounts.ToListAsync();
      var symbols = await context.Symbols.ToListAsync();
      var random = new Random();

      foreach (var account in accounts)
      {
         var numberOfTrades = random.Next(300, 600);
         var tradesPerMonth = numberOfTrades / 12;

         for (var m = 0; m < numberOfTrades; m++)
         {
            var openedAt = DateTime.Now.AddDays(-random.Next(0, 365)).AddMinutes(-random.Next(10, 1000));
            var closedAt = openedAt.AddHours(random.Next(100));

            var trade = new Trade()
            {
               SymbolId = symbols[m % symbols.Count].Id,
               Side = random.Next(100) < 50 ? TradeSide.Short : TradeSide.Long,
               TradingAccount = account,
               OpenedAt = openedAt,
               ClosedAt = closedAt,
               IsHidden = random.Next(100) < 2,
               Confluences = random.Next(1, 10),
            };

            decimal size = random.Next(10, 999);
            var price = random.Next(1, 100) / 10m;
            var fee = random.Next(100) / 80m;

            var openingExecution = new Execution()
            {
               Action = trade.Side == TradeSide.Short ? TradeAction.Sell : TradeAction.Buy,
               ExecutedAt = openedAt,
               Size = size,
               Price = price,
               Fee = fee,
               Position = size,
               Value = size * price,
               Direction = TradeDirection.Open,
            };

            trade.AverageEntryPrice = price;
            trade.Size = size;

            price = price + random.Next(1, 10) / 10m - random.Next(1, 10) / 10m;
            var total = size * price;

            var closingExecution = new Execution()
            {
               Action = trade.Side == TradeSide.Short ? TradeAction.Buy : TradeAction.Sell,
               ExecutedAt = openedAt,
               Size = size,
               Price = price,
               Fee = fee,
               Position = 0,
               Value = size * price,
               Direction = TradeDirection.Close,
            };

            trade.AverageExitPrice = price;
            var totalFees = openingExecution.Fee + closingExecution.Fee;

            if (trade.Side == TradeSide.Short)
            {
               closingExecution.Return = trade.Return = (trade.AverageEntryPrice - trade.AverageExitPrice) * trade.Size;
               closingExecution.NetReturn = trade.NetReturn = trade.Return - totalFees;
            }
            else
            {
               closingExecution.Return = trade.Return = (trade.AverageExitPrice - trade.AverageEntryPrice) * trade.Size;
               closingExecution.NetReturn = trade.NetReturn = trade.Return - totalFees;
            }

            trade.Status = trade.NetReturn switch
            {
               > 0 => TradeStatus.Win,
               < 0 => TradeStatus.Loss,
               _ => TradeStatus.Breakeven,
            };

            trade.Executions.Add(openingExecution);
            trade.Executions.Add(closingExecution);

            context.Trades.Add(trade);
         }

         await context.SaveChangesAsync();
      }
   }

   private static async Task SeedSymbols(ApplicationDbContext context)
   {
      context.Symbols.Add(new Symbol() { Name = "MATICUSDT" });
      context.Symbols.Add(new Symbol() { Name = "LTCUSDT" });
      context.Symbols.Add(new Symbol() { Name = "ETHUSDT" });
      context.Symbols.Add(new Symbol() { Name = "SANDUSDT" });
      context.Symbols.Add(new Symbol() { Name = "SOLUSDT" });
      context.Symbols.Add(new Symbol() { Name = "GALAUSDT" });
      context.Symbols.Add(new Symbol() { Name = "AVAXUSDT" });
      context.Symbols.Add(new Symbol() { Name = "ADAUSDT" });
      context.Symbols.Add(new Symbol() { Name = "LUNAUSDT" });
      context.Symbols.Add(new Symbol() { Name = "FTMUSDT" });

      await context.SaveChangesAsync();
   }

   private static async Task SeedTradingAccounts(ApplicationDbContext context)
   {
      var trader1 = context.Users.FirstOrDefault(x => x.DisplayName == "Trader1");

      var account1 = new TradingAccount { Name = "MainAccount", APIKey = "O81W5WnFeXlfVEe1BB", APISecret = "GN8zk27a7fS6vCweTgyxgtobYi6LfEfQUfYF", User = trader1, Status = TradingAccountStatus.Disabled };
      var account2 = new TradingAccount { Name = "LearningAccount", APIKey = "XAXWVXPNTPLUMJRHOI", APISecret = "TOCXLKXQBDWJADFYFHHKINDADLNHOKMPYQTE", User = trader1, Status = TradingAccountStatus.Disabled };

      // disabled activation of trading accounts as they are IP restricted and not needed to be active for the demo data
      // account1.DomainEvents.Add(new TradingAccountActivated(account1));
      // account2.DomainEvents.Add(new TradingAccountActivated(account2));

      context.TradingAccounts.Add(account1);
      context.TradingAccounts.Add(account2);

      await context.SaveChangesAsync();
   }

   private static async Task SeedUsers(ApplicationDbContext context)
   {
      var users = new List<User>()
            {
                new User{ DisplayName = "Trader1", Email = "Trader1@example.com", PasswordHash = StringToByteArray("0,134,8,193,5,224,185,107,43,86,115,249,74,237,223,200,157,13,155,10,108,13,160,218,6,192,68,194,241,49,133,85,209,15,43,246,202,88,25,240,205,138,121,110,218,9,252,73,163,196,118,184,71,178,156,5,105,54,95,252,89,196,212,65"), PasswordSalt = StringToByteArray("50,177,155,182,93,251,236,250,228,83,228,143,85,37,48,67,0,85,152,188,153,81,216,244,94,8,107,239,114,119,89,223,53,235,12,54,206,203,34,1,185,144,24,186,78,107,241,147,93,151,185,188,242,120,83,235,158,221,175,93,228,141,166,60,140,238,153,28,253,136,42,190,157,172,29,165,177,66,244,59,20,222,209,40,48,51,200,21,32,81,212,74,144,23,101,152,238,93,197,218,200,134,220,219,216,238,253,118,35,76,165,49,240,91,94,84,40,69,163,50,55,227,149,27,36,242,180,234") },
                new User{ DisplayName = "Trader2", Email = "Trader2@example.com", PasswordHash = StringToByteArray("64,148,124,233,251,246,69,85,127,63,173,116,111,82,14,44,203,2,247,104,110,87,218,24,72,58,211,75,60,165,8,27,140,153,6,117,77,10,182,232,81,224,11,14,254,103,51,40,196,137,30,67,199,85,217,109,178,169,216,155,147,149,192,103"), PasswordSalt = StringToByteArray("137,26,213,114,168,127,99,244,220,128,31,208,106,208,4,58,233,151,82,186,92,22,44,123,105,105,51,173,113,43,167,185,144,154,228,203,39,68,12,143,49,200,108,36,20,229,52,145,64,147,234,203,53,58,72,79,79,0,64,165,198,159,85,201,134,115,1,198,234,17,88,255,65,119,222,124,26,118,77,3,53,2,219,58,69,56,171,35,201,32,93,95,46,143,118,200,60,185,96,218,118,144,18,0,21,237,217,46,109,142,117,237,55,125,171,41,188,110,50,36,21,181,60,130,224,166,144,139") },
            };

      foreach (var user in users)
         context.Users.Add(user);

      await context.SaveChangesAsync();
   }

   private static byte[] StringToByteArray(string input, char seperator = ',')
   {
      return input.Split(seperator)
          .Select(x => byte.Parse(x))
          .ToArray();
   }
}
