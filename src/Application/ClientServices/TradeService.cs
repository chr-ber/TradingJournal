using TradingJournal.Application.Accounts.Commands.ChangeTradingAccountState;
using TradingJournal.Application.Accounts.Commands.CreateTradingAccount;
using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Application.Common.Models;
using TradingJournal.Domain.Entities;
using TradingJournal.Domain.Enums;
using Blazored.Toast.Services;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TradingJournal.Application.CQS.Trades.Commands;

namespace TradingJournal.Application.ClientServices;

public class TradeService : ITradeService
{
    private readonly HttpClient _http;
    private readonly IToastService _toastService;

    public PaginatedList<Trade> PaginatedList { get; set; }

    public TradeService(HttpClient http, IToastService toastService)
    {
        _http = http;
        _toastService = toastService;
    }

    public async Task LoadTrades(
        int pageNumber = 1,
        int pageSize = 10,
        IEnumerable<TradeStatus> includedStates = null)
    {
        //TradingAccounts = await _http.GetFromJsonAsync<PaginatedList<TradingAccount>>("api/accounts");

        Dictionary<string, string> query = new()
        {
            ["pageNumber"] = pageNumber.ToString(),
            ["pageSize"] = pageSize.ToString(),
        };

        if(includedStates != null)
            foreach(var state in includedStates)
                query["includedStates"] = ((int)state).ToString();

        var dictFormUrlEncoded = new FormUrlEncodedContent(query);
        var queryString = await dictFormUrlEncoded.ReadAsStringAsync();

        Console.WriteLine(queryString);

        string json = await _http.GetStringAsync($"api/trades?{queryString}");

        JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        var list = JsonSerializer.Deserialize<PaginatedList<Trade>>(json, options);
        PaginatedList = list;
    }

    public async Task DeleteTrade(int id)
    {
        throw new NotImplementedException();
        //await _http.DeleteAsync($"api/trades/{id}");
    }

    public async Task UpdateJournalFields(UpdateJournalingFieldsCommand command)
    {
        await _http.PutAsJsonAsync("api/trades/journal", command);
    }

    public async Task<Trade> GetTrade(int id)
    {
        string json = await _http.GetStringAsync($"api/trades/{id}");

        JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<Trade>(json, options);
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

    public async Task<float> GetWinLossRatio()
    {
        throw new NotImplementedException();
    }
}
