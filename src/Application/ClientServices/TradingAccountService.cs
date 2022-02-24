﻿using TradingJournal.Application.TradingAccounts.Commands.CreateTradingAccount;
using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Application.Common.Models;
using TradingJournal.Domain.Entities;
using TradingJournal.Domain.Enums;
using Blazored.Toast.Services;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TradingJournal.Domain.Entities;
using TradingJournal.Application.TradingAccounts.Commands.ChangeTradingAccountState;

namespace TradingJournal.Application.ClientServices;

public class TradingAccountService : ITradingAccountService
{
    private readonly HttpClient _http;
    private readonly IToastService _toastService;

    public PaginatedList<TradingAccount> List { get; set; }

    public TradingAccountService(HttpClient http, IToastService toastService)
    {
        _http = http;
        _toastService = toastService;
    }

    public async Task LoadTradingAccounts(int pageNumber = 1, int pageSize = 10)
    {
        //TradingAccounts = await _http.GetFromJsonAsync<PaginatedList<TradingAccount>>("api/tradingaccounts");

        Dictionary<string, string> query = new()
        {
            ["pageNumber"] = pageNumber.ToString(),
            ["pageSize"] = pageSize.ToString(),
        };

        var dictFormUrlEncoded = new FormUrlEncodedContent(query);
        var queryString = await dictFormUrlEncoded.ReadAsStringAsync();

        string json = await _http.GetStringAsync($"api/tradingaccounts?{queryString}");

        JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        var list = JsonSerializer.Deserialize<PaginatedList<TradingAccount>>(json, options);
        List = list;
    }

    public async Task DeleteTradingAccount(int id)
    {
        await _http.DeleteAsync($"api/tradingaccounts/{id}");
    }

    public async Task SetStatus(int id, TradingAccountStatus status)
    {
        var request = new SetTradingAccountStateCommand { Id = id, Status = status };
        await _http.PutAsJsonAsync("api/tradingaccounts/set-status", request);
    }

    public async Task<ServiceResponse<int>> AddTradingAccount(CreateTradingAccountCommand command)
    {
        var result = await _http.PostAsJsonAsync($"api/tradingaccounts/", command);

        var response = new ServiceResponse<int>() 
        {
            Success = result.StatusCode == HttpStatusCode.OK,
        };

        string validationErrors = string.Empty;
        if(result.StatusCode == HttpStatusCode.BadRequest)
        {
            var contents = await result.Content.ReadAsStringAsync();
            validationErrors = JsonSerializer.Deserialize<ValidationErrorResponse>(contents).ToString();
        }

        response.Message = result.StatusCode switch
        {
            HttpStatusCode.OK => "Added Trading Account successfully.",
            HttpStatusCode.InternalServerError => "Unable to add trading account, please verify that api key and secret are correct and not expired.",
            HttpStatusCode.BadRequest => validationErrors,
            _ => "Unable to create account, please try again later.",
        };

        return response;
    }

}
