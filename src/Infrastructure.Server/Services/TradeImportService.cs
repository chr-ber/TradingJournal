using Microsoft.Extensions.Hosting;

namespace TradingJournal.Infrastructure.Server.Services;

public class TradeImportService : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while(cancellationToken.IsCancellationRequested is false)
        {

        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}