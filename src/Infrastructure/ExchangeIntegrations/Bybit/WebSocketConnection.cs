namespace TradingJournal.Infrastructure.ExchangeIntegrations.Bybit;

public class WebSocketConnection : IDisposable
{
   private readonly WebsocketClient _client;
   private readonly CancellationToken _cancellationToken;

   public Network Network { get; }
   public ContractEndpoint Endpoint { get; }

   public string ApiKey { get; init; }

   public string ApiSecret { get; init; }

   public bool IsRunning => _client.IsRunning;

   public event Func<List<WebSocketExecution>, Task> ExecutionEventOccured;

   public WebSocketConnection(
       CancellationToken cancellationToken,
       ContractEndpoint endpoint = ContractEndpoint.USDTPerpetual,
       Network network = Network.TestNet)
   {
      _cancellationToken = cancellationToken;

      Network = network;
      Endpoint = endpoint;

      var url = endpoint switch
      {
         ContractEndpoint.InversePerpetual => "wss://stream-testnet.bybit.com/realtime",
         ContractEndpoint.USDTPerpetual => "wss://stream-testnet.bybit.com/realtime_private",
         _ => throw new NotImplementedException(),
      };

      var exitEvent = new ManualResetEvent(false);

      _client = new WebsocketClient(new Uri(url));
      _client.ReconnectTimeout = TimeSpan.FromSeconds(30);
      _client.ReconnectionHappened.Subscribe(info =>
      {
         System.Diagnostics.Debug.WriteLine($"Reconnection happened, type: {info.Type}");

         // if connection was lost -> reconnect
         if (info.Type != ReconnectionType.Initial)
         {
            Connect();
         }
      });

      _client.MessageReceived
          .Where(msg => msg.Text != null)
          .Where(msg => msg.Text.Contains("auth"))
          .Subscribe(obj =>
          {
             System.Diagnostics.Debug.WriteLine($"\r\n{DateTime.Now} - {Endpoint} - Message received:\r\n{obj}");
          });

      _client.MessageReceived
          .Where(msg => msg.Text != null)
          .Where(msg => msg.Text.StartsWith("{\"topic\""))
          .Subscribe(obj =>
          {
             System.Diagnostics.Debug.WriteLine($"\r\n{DateTime.Now} - {Endpoint} - Message received:\r\n{obj}");

             var options = new JsonSerializerOptions();
             options.Converters.Add(new JsonStringEnumConverter());

             var message = JsonSerializer.Deserialize<WebSocketMessage>(obj.Text, options);

             if (message.topic == Topic.Execution)
             {
                var messageData = JsonSerializer.Deserialize<WebSocketMessage<WebSocketExecution>>(obj.Text, options).data;
                ExecutionEventOccured?.Invoke(messageData.ToList());
             }
          });
   }

   public async void Connect()
   {
      await _client.Start();

      await SendAuthentication();
      await SendSubscriptionMessage();

      System.Diagnostics.Debug.WriteLine($"{DateTime.Now} - Connected {ApiKey} with {Endpoint} endpoint on {Network}");

      while (_client.IsRunning && !_cancellationToken.IsCancellationRequested)
      {
         Thread.Sleep(28 * 1000);
         _client.Send("{\"op\":\"ping\"}");
      }
      System.Diagnostics.Debug.WriteLine($"{DateTime.Now} - Closed Websocket client {Endpoint} endpoint on {Network}");
   }

   private async Task SendAuthentication()
   {
      System.Diagnostics.Debug.WriteLine($"{DateTime.Now} - Authenticating {Endpoint} endpoint on {Network}");

      var expiresIn = GetExpirationInUnixMilliseconds();
      var signature = ApiUtilityService.CreateSignature(ApiSecret, $"GET/realtime{expiresIn}");
      var authString = $"{{ \"op\" : \"auth\" , \"args\" : [ \"{ApiKey}\" , \"{expiresIn}\" , \"{signature}\" ] }}";

      await _client.SendInstant(authString);
   }

   private async Task SendSubscriptionMessage()
   {
      var subscriptionMessage = "{\"op\":\"subscribe\",\"args\":[\"execution\", \"position\"]}";
      await _client.SendInstant(subscriptionMessage);
   }

   public void Dispose()
   {
      _client.Dispose();
   }

   public static long GetExpirationInUnixMilliseconds() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 5000;
}
