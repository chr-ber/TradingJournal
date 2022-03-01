using Microsoft.EntityFrameworkCore;
using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Domain.Entities;
using TradingJournal.Domain.Enums;
using MediatR;
using static TradingJournal.Application.CQS.Reports.Queries.GetDailyReport.DailyReportDto;

namespace TradingJournal.Application.CQS.Reports.Queries.GetDailyReport;

public class GetDailyReportQuery : IRequest<DailyReportDto>
{

}

public class GetDailyReportQueryHandler : IRequestHandler<GetDailyReportQuery, DailyReportDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetDailyReportQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<DailyReportDto> Handle(GetDailyReportQuery request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserId();

        var trades = await _context.Trades
            .Where(x => x.OpenedAt > DateTime.UtcNow.AddDays(-365))
            .Where(x => x.TradingAccount.UserId == userId)
            .Where(x => x.Status != TradeStatus.Open)
            .ToListAsync();

        var groupedTrades = trades
            .OrderBy(x => ((int) x.OpenedAt.DayOfWeek + 6 % 7)) // as sunday has value of 0 add 6 to make it end of week
            .GroupBy(x => x.OpenedAt.DayOfWeek);

        var report = new DailyReportDto();

        double totalTradesTaken = trades.Count();
        double highestWinRatio = double.MinValue;
        double highestLossRatio = double.MinValue;

        foreach (var group in groupedTrades)
        {
            var weekday = new WeekDayStatistics()
            {
                Weekday = group.Key.ToString(),
                WinPercentage = group.Count(x => x.Status == TradeStatus.Win) / totalTradesTaken,
                LoosePercentage = group.Count(x => x.Status == TradeStatus.Loss) / totalTradesTaken,
                BreakEvenPercentage = group.Count(x => x.Status == TradeStatus.Breakeven) / totalTradesTaken,
                TotalTradesTaken = group.Count(),
            };

            report.WeekdayStats.Add(weekday);

            if(weekday.WinPercentage > highestWinRatio)
            {
                report.BestPerformingDay = group.Key.ToString();
                highestWinRatio = weekday.WinPercentage;
            }

            if (weekday.LoosePercentage > highestLossRatio)
            {
                report.WorstPerfromingDay = group.Key.ToString();
                highestLossRatio = weekday.LoosePercentage;
            }
        }

        report.WeekdayChart = new()
        {
            new() { Name = "Win", Data = report.WeekdayStats.Select(x => x.WinPercentage * 100).ToArray() },
            new() { Name = "Loss", Data = report.WeekdayStats.Select(x => x.LoosePercentage * 100).ToArray() },
            new() { Name = "Break Even", Data = report.WeekdayStats.Select(x => x.BreakEvenPercentage * 100).ToArray() },
        };

        

        return report;
    }
}