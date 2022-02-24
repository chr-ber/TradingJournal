using TradingJournal.Application.Common.Models;

namespace TradingJournal.Infrastructure.Server.ExchangeIntegrations.Bybit;

public interface IByBitApiWrapper : IDisposable
{
    string ApiKey { get; set; }

    string ApiSecret { get; set; }

    event Func<List<WebSocketExecution>, IByBitApiWrapper, bool, decimal, Task> OnReceivedExecution;

    CancellationToken stoppingToken { get; set; }

    void CreateWebSocketConnection();
}
