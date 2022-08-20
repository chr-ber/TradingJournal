namespace TradingJournal.Application.ClientServices;

public class ReportService : ClientServiceBase, IReportService
{
   private readonly HttpClient _http;

   public ReportService(HttpClient http)
   {
      _http = http;
   }

   public async Task<DayOfWeekReportDto> GetDayOfWeekReport()
   {
      var response = await _http.GetStringAsync("api/reports/dayofweek");

      return JsonSerializer.Deserialize<DayOfWeekReportDto>(response, _jsonOptions);
   }

   public async Task<MonthReportDto> GetMonthReport()
   {
      var response = await _http.GetStringAsync("api/reports/month");

      return JsonSerializer.Deserialize<MonthReportDto>(response, _jsonOptions);
   }
}
