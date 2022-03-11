using TradingJournal.Infrastructure.Server.ExchangeIntegrations.Bybit.Enums;
using TradingJournal.Infrastructure.Server.ExchangeIntegrations.Bybit.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Diagnostics;

namespace TradingJournal.Infrastructure.Server.ExchangeIntegrations.Bybit;

public class ByBitApiWrapper
{
    private WebSocketConnection _wsConnectionInverse;
    private WebSocketConnection _wsConnectionUsdt;

    public event Func<List<WebSocketExecution>, ByBitApiWrapper, bool, decimal, Task> OnReceivedExecution;

    public string ApiKey { get; set; }

    public string ApiSecret { get; set; }

    public CancellationToken stoppingToken { get; set; }

    public void CreateWebSocketConnection()
    {
        _wsConnectionInverse = new(stoppingToken, ContractEndpoint.InversePerpetual)
        {
            APIKey = this.ApiKey,
            APISecret = this.ApiSecret,
        };

        _wsConnectionInverse.Connect();
        //_wsConnectionInverse.ExecutionEventOccured += GetRestExecutionsRecordForWebsocketExecution;

        _wsConnectionUsdt = new(stoppingToken, ContractEndpoint.USDTPerpetual)
        {
            APIKey = this.ApiKey,
            APISecret = this.ApiSecret,
        };

        _wsConnectionUsdt.Connect();
        //_wsConnectionUsdt.ExecutionEventOccured += OnReceivedExecution;
        _wsConnectionUsdt.ExecutionEventOccured += OnLinearExecution;
    }

    public async Task OnLinearExecution(List<WebSocketExecution> execution)
    {
        var executionGrped = execution.GroupBy(x => x.order_id);

        // short delay that data is available via rest api
        await Task.Delay(1 * 2000);

        // process each grp of executions by order_id
        foreach (var grp in executionGrped)
        {
            var item = grp.FirstOrDefault();

            // get detailed order information from rest api
            var order = await GetActiveLinearOrderByOrderId(item.order_id, item.symbol);

            // if no order returned, retry in a few seconds
            if (order is null)
            {
                await Task.Delay(1 * 10000);
                order = await GetActiveLinearOrderByOrderId(item.order_id, item.symbol);
            }

            // Trigger event for grp of executions and bool if order was opened or closed (closed = reduce_only)
            OnReceivedExecution?.Invoke(grp.ToList(), this, order.reduce_only, order.qty);
        }
    }

    public async Task<LinearOrderResult> GetActiveLinearOrderByOrderId(string orderId, string symbol)
    {
        var timestamp = UtilityService.GetExpirationInUnixMilliseconds();

        string queryParams = $"api_key={ApiKey}&order_id={orderId}&symbol={symbol}&timestamp={timestamp}";

        string signature = UtilityService.CreateSignature(ApiSecret, queryParams);

        using (var client = new HttpClient())
        {
            string requestUrl = "https://api-testnet.bybit.com/private/linear/order/search?" + $"{queryParams}&sign={signature}";

            var content = await client.GetStringAsync(requestUrl);

            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            try
            {
                var executions = JsonSerializer.Deserialize<LinearOrderResultBase>(content, options);
                return executions.result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Debug.WriteLine(content);
                return null;
            }
        }
    }

    public async Task<long> GetServerTime()
    {
        using(var client = new HttpClient())
        {
            var response = await client.GetAsync("https://api-testnet.bybit.co/v2/public/time");

            throw new NotImplementedException();
        }
    }

    // execution records that are received via websocket will be avialable 10-30 seconds later via rest api
    // the websocket version has less details and is not sufficient for an import
    public async Task GetRestExecutionRecordsForWebsocketExecution(WebSocketExecution socketExecution)
    {
        //await Task.Delay(1000 * 10);
        int requestCount = 0;
        int maxRetries = 3;
        int delayInMilliseconds = 10000;

        List<RestApiExecution> restExecution;

        Debug.WriteLine($"Searching rest execution for order {socketExecution.order_id}.");

        do
        {
            requestCount++;
            await Task.Delay(delayInMilliseconds);
            List<RestApiExecution> executions = await GetUserExecutionRecords(socketExecution.symbol);

            restExecution = executions.Where(x => x.order_id == socketExecution.order_id).ToList();

        } while (!restExecution.Any() && requestCount == maxRetries);

        if (!restExecution.Any())
        {
            throw new Exception($"Could not find matching execution within delay {maxRetries * delayInMilliseconds / 1000}.");
        }

        Debug.WriteLine($"Found matching execution(s) via rest api. {restExecution.Count}");
    }

    public async Task<List<RestApiExecution>> GetUserExecutionRecords(string symbol)
    {
        var timestamp = UtilityService.GetExpirationInUnixMilliseconds();

        string signature = UtilityService.CreateSignature(ApiSecret, $"api_key={ApiKey}&symbol={symbol}&timestamp={timestamp}");

        using (var client = new HttpClient())
        {
            string requestUrl = "https://api-testnet.bybit.com/private/linear/trade/execution/list?" + $"api_key={ApiKey}&symbol={symbol}&timestamp={timestamp}&sign={signature}";

            var content = await client.GetStringAsync(requestUrl);

            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            try
            {
                var executions = JsonSerializer.Deserialize<ApiResponse<RestApiExecution>>(content, options);
                return executions.result.data.ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Debug.WriteLine(content);
                return null;
            }
        }
    }

    public void Dispose()
    {
        //_wsConnectionInverse.Dispose();
        //_wsConnectionUsdt.Dispose();
    }
}
