using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Application.CQS.Reports.Queries.GetDailyReport;
using System.Text.Json;

namespace TradingJournal.Application.ClientServices;

public class ReportService : IReportService
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _jsonOptions;

    public ReportService(HttpClient http)
    {
        _http = http;

        _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<DailyReportDto> GetDailyReport()
    {
        string response = await _http.GetStringAsync("api/reports/daily");

        return JsonSerializer.Deserialize<DailyReportDto>(response, _jsonOptions);
    }

    public async Task<DailyReportDto> GetWeeklyReport()
    {
        string response = await _http.GetStringAsync("api/reports/daily");

        return JsonSerializer.Deserialize<DailyReportDto>(response, _jsonOptions);
    }

    public async Task<DailyReportDto> GetMonthlyReport()
    {
        string response = await _http.GetStringAsync("api/reports/daily");

        return JsonSerializer.Deserialize<DailyReportDto>(response, _jsonOptions);
    }
}