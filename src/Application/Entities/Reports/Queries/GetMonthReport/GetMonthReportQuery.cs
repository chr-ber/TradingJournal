using Microsoft.EntityFrameworkCore;
using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Domain.Entities;
using TradingJournal.Domain.Enums;
using MediatR;
using System.Globalization;

namespace TradingJournal.Application.Entities.Reports.Queries.GetMonthReportQuery;

public class GetMonthReportQuery : IRequest<MonthReportDto>
{

}

public class GetMonthReportQueryHandler : IRequestHandler<GetMonthReportQuery, MonthReportDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetMonthReportQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<MonthReportDto> Handle(GetMonthReportQuery request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserId();

        // get all trades within the last year that have been closed
        var trades = await _context.Trades
            .Where(x => x.OpenedAt > DateTime.UtcNow.AddDays(-365))
            .Where(x => x.TradingAccount.UserId == userId)
            .Where(x => x.Status != TradeStatus.Open)
            .ToListAsync();

        // group trades by month
        var groupedTrades = trades
            .OrderBy(x => x.OpenedAt.Month)
            .GroupBy(x => x.OpenedAt.Month)
            .ToList();

        var report = new MonthReportDto();

        double totalTradesTaken = trades.Count();
        double highestWinRatio = double.MinValue;
        double highestLossRatio = double.MinValue;

        for (int i = 1; i <= 12; i++)
        {
            int currentMonth = (DateTime.UtcNow.Month + i - 1) % 12 + 1;

            var group = groupedTrades.FirstOrDefault(x => x.Key == currentMonth);
            var monthName = new DateTime(2010, currentMonth, 1).ToString("MMM", CultureInfo.InvariantCulture);

            MonthStatistics monthStatistics;

            // if no trades took place in the looped month
            if (group is null)
            {
                monthStatistics = new MonthStatistics()
                {
                    Month = monthName,
                    WinPercentage = 0,
                    LoosePercentage = 0,
                    BreakEvenPercentage = 0,
                    TotalTradesTaken = 0,
                };
            }
            else
            {
                monthStatistics = new MonthStatistics()
                {
                    Month = monthName,
                    WinPercentage = group.Count(x => x.Status == TradeStatus.Win) / totalTradesTaken,
                    LoosePercentage = group.Count(x => x.Status == TradeStatus.Loss) / totalTradesTaken,
                    BreakEvenPercentage = group.Count(x => x.Status == TradeStatus.Breakeven) / totalTradesTaken,
                    TotalTradesTaken = group.Count(),
                };
            }

            report.MonthStatistics.Add(monthStatistics);

            if (monthStatistics.WinPercentage > highestWinRatio)
            {
                report.BestPerformingMonth = monthStatistics.Month;
                highestWinRatio = monthStatistics.WinPercentage;
            }

            if (monthStatistics.LoosePercentage > highestLossRatio)
            {
                report.WorstPerfromingMonth = monthStatistics.Month;
                highestLossRatio = monthStatistics.LoosePercentage;
            }
        }

        // create the chartseries for the mudblzor chart
        report.MonthChart = new()
        {
            new() { Name = "Win", Data = report.MonthStatistics.Select(x => x.WinPercentage * 100).ToArray() },
            new() { Name = "Loss", Data = report.MonthStatistics.Select(x => x.LoosePercentage * 100).ToArray() },
            new() { Name = "Break Even", Data = report.MonthStatistics.Select(x => x.BreakEvenPercentage * 100).ToArray() },
        };

        return report;
    }
}