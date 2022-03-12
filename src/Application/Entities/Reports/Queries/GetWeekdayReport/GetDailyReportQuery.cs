using Microsoft.EntityFrameworkCore;
using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Domain.Entities;
using TradingJournal.Domain.Enums;
using MediatR;
using static TradingJournal.Application.Entities.Reports.Queries.GetDailyReport.DayOfWeekReportDto;

namespace TradingJournal.Application.Entities.Reports.Queries.GetDailyReport;

public class GetDayOfWeekReportQuery : IRequest<DayOfWeekReportDto>
{

}

public class GetDayOfWeekReportQueryHandler : IRequestHandler<GetDayOfWeekReportQuery, DayOfWeekReportDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetDayOfWeekReportQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<DayOfWeekReportDto> Handle(GetDayOfWeekReportQuery request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserId();

        // get all trades within the last year that have been closed
        var trades = await _context.Trades
            .Where(x => x.OpenedAt > DateTime.UtcNow.AddDays(-365))
            .Where(x => x.TradingAccount.UserId == userId)
            .Where(x => x.Status != TradeStatus.Open)
            .ToListAsync();

        // group trades by day of week
        var groupedTrades = trades
            .OrderBy(x => ((int) x.OpenedAt.DayOfWeek + 6 % 7)) // as sunday has value of 0 add 6 to make it end of week
            .GroupBy(x => x.OpenedAt.DayOfWeek);

        var report = new DayOfWeekReportDto();

        double totalTradesTaken = trades.Count();
        double highestWinRatio = double.MinValue;
        double highestLossRatio = double.MinValue;

        for(int i = 0; i < 7; i++)
        {
            int currentDay = (i + 1) % 7; // as sunday has value of 0 add 6 to make it end of week
            var group = groupedTrades.FirstOrDefault(x => (int)x.Key == currentDay);

            DayOfWeekStatistics dayofweekStatistics;

            // create empty if no trades took place on the given day
            if (group is null)
            {
                dayofweekStatistics = new DayOfWeekStatistics()
                {
                    DayOfWeek = ((DayOfWeek)currentDay).ToString(),
                    WinPercentage = 0,
                    LoosePercentage = 0,
                    BreakEvenPercentage = 0,
                    TotalTradesTaken = 0,
                };
            }
            else
            {
                dayofweekStatistics = new DayOfWeekStatistics()
                {
                    DayOfWeek = group.Key.ToString(),
                    WinPercentage = group.Count(x => x.Status == TradeStatus.Win) / totalTradesTaken,
                    LoosePercentage = group.Count(x => x.Status == TradeStatus.Loss) / totalTradesTaken,
                    BreakEvenPercentage = group.Count(x => x.Status == TradeStatus.Breakeven) / totalTradesTaken,
                    TotalTradesTaken = group.Count(),
                };
            }

            report.MonthStatistics.Add(dayofweekStatistics);

            if(dayofweekStatistics.WinPercentage > highestWinRatio)
            {
                report.BestPerformingDay = dayofweekStatistics.DayOfWeek;
                highestWinRatio = dayofweekStatistics.WinPercentage;
            }

            if (dayofweekStatistics.LoosePercentage > highestLossRatio)
            {
                report.WorstPerfromingDay = dayofweekStatistics.DayOfWeek;
                highestLossRatio = dayofweekStatistics.LoosePercentage;
            }
        }

        // create the chartseries for the mudblzor chart
        report.DayOfWeekChart = new()
        {
            new() { Name = "Win", Data = report.MonthStatistics.Select(x => x.WinPercentage * 100).ToArray() },
            new() { Name = "Loss", Data = report.MonthStatistics.Select(x => x.LoosePercentage * 100).ToArray() },
            new() { Name = "Break Even", Data = report.MonthStatistics.Select(x => x.BreakEvenPercentage * 100).ToArray() },
        };

        return report;
    }
}