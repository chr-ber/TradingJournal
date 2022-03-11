using Microsoft.EntityFrameworkCore;
using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Domain.Entities;
using TradingJournal.Domain.Enums;
using MediatR;
using static TradingJournal.Application.Entities.Reports.Queries.GetDailyReport.WeekdayReportDto;

namespace TradingJournal.Application.Entities.Reports.Queries.GetDailyReport;

public class GetWeekdayReportQuery : IRequest<WeekdayReportDto>
{

}

public class GetWeekdayReportQueryHandler : IRequestHandler<GetWeekdayReportQuery, WeekdayReportDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetWeekdayReportQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<WeekdayReportDto> Handle(GetWeekdayReportQuery request, CancellationToken cancellationToken)
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

        var report = new WeekdayReportDto();

        double totalTradesTaken = trades.Count();
        double highestWinRatio = double.MinValue;
        double highestLossRatio = double.MinValue;

        for(int i = 0; i < 7; i++)
        {
            int currentDay = (i + 1) % 7; // as sunday has value of 0 add 6 to make it end of week
            var group = groupedTrades.FirstOrDefault(x => (int)x.Key == currentDay);

            WeekDayStatistics weekdayStatistics;

            if (group is null)
            {
                weekdayStatistics = new WeekDayStatistics()
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
                weekdayStatistics = new WeekDayStatistics()
                {
                    DayOfWeek = group.Key.ToString(),
                    WinPercentage = group.Count(x => x.Status == TradeStatus.Win) / totalTradesTaken,
                    LoosePercentage = group.Count(x => x.Status == TradeStatus.Loss) / totalTradesTaken,
                    BreakEvenPercentage = group.Count(x => x.Status == TradeStatus.Breakeven) / totalTradesTaken,
                    TotalTradesTaken = group.Count(),
                };
            }

            report.MonthStatistics.Add(weekdayStatistics);

            if(weekdayStatistics.WinPercentage > highestWinRatio)
            {
                report.BestPerformingDay = weekdayStatistics.DayOfWeek;
                highestWinRatio = weekdayStatistics.WinPercentage;
            }

            if (weekdayStatistics.LoosePercentage > highestLossRatio)
            {
                report.WorstPerfromingDay = weekdayStatistics.DayOfWeek;
                highestLossRatio = weekdayStatistics.LoosePercentage;
            }
        }

        report.WeekdayChart = new()
        {
            new() { Name = "Win", Data = report.MonthStatistics.Select(x => x.WinPercentage * 100).ToArray() },
            new() { Name = "Loss", Data = report.MonthStatistics.Select(x => x.LoosePercentage * 100).ToArray() },
            new() { Name = "Break Even", Data = report.MonthStatistics.Select(x => x.BreakEvenPercentage * 100).ToArray() },
        };

        return report;
    }
}