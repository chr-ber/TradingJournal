using TradingJournal.Application.Entities.Trades.Commands.UpdateJournalingFields;
using TradingJournal.Application.Entities.Trades.Commands.HideTrade;
using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Application.Common.Models;
using TradingJournal.Domain.Entities;
using TradingJournal.Domain.Enums;
using System.Net.Http.Json;
using System.Text.Json;

namespace TradingJournal.Application.ClientServices;

public class TradeService : ClientServiceBase, ITradeService
{
    private readonly HttpClient _http;

    public PaginatedList<Trade> PaginatedList { get; set; }

    public TradeService(HttpClient http)
    {
        _http = http;
    }

    public async Task LoadTrades(
        int pageNumber = 1,
        int pageSize = 10,
        IEnumerable<TradeStatus> includedStates = null,
        bool hidden = false)
    {
        Dictionary<string, string> query = new()
        {
            ["pageNumber"] = pageNumber.ToString(),
            ["pageSize"] = pageSize.ToString(),
            ["hidden"] = hidden.ToString(),
        };

        if(includedStates != null)
            foreach(var state in includedStates)
                query["includedStates"] = ((int)state).ToString();

        var dictFormUrlEncoded = new FormUrlEncodedContent(query);
        var queryString = await dictFormUrlEncoded.ReadAsStringAsync();

        string json = await _http.GetStringAsync($"api/trades?{queryString}");

        var list = JsonSerializer.Deserialize<PaginatedList<Trade>>(json, _jsonOptions);
        PaginatedList = list;
    }

    public async Task UpdateJournalFields(UpdateJournalingFieldsCommand command)
    {
        await _http.PutAsJsonAsync("api/trades/journal", command);
    }

    public async Task<Trade> GetTrade(int id)
    {
        string json = await _http.GetStringAsync($"api/trades/{id}");

        return JsonSerializer.Deserialize<Trade>(json, _jsonOptions);
    }

    public async Task<int> GetTradeCount()
    {
        string response =  await _http.GetStringAsync("api/trades/count");

        return int.Parse(response);
    }

    public async Task<decimal> GetAverageReturn()
    {
        string response = await _http.GetStringAsync("api/trades/average-return");

        return decimal.Parse(response);
    }


    public async Task BatchSetVisibility(IEnumerable<int> ids, bool isHidden)
    {
        var command = new BatchSetTradeVisibilityCommand { Ids = ids, IsHidden = isHidden };
        var response = await _http.PutAsJsonAsync("api/trades/set-visibility", command);
    }
}
