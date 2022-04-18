using TradingJournal.Application.Entities.Accounts.Commands.ChangeTradingAccountState;
using TradingJournal.Application.Entities.Accounts.Commands.CreateTradingAccount;
using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Application.Common.Models;
using TradingJournal.Domain.Entities;
using TradingJournal.Domain.Enums;
using System.Net.Http.Json;
using System.Text.Json;
using System.Net;

namespace TradingJournal.Application.ClientServices;

public class TradingAccountService : ClientServiceBase, ITradingAccountService
{
    private readonly HttpClient _http;

    public PaginatedList<TradingAccount> List { get; set; }

    public TradingAccountService(HttpClient http)
    {
        _http = http;
    }

    public async Task LoadTradingAccounts(int pageNumber = 1, int pageSize = 10)
    {
        // create a dictionary for all query parameters
        Dictionary<string, string> query = new()
        {
            ["pageNumber"] = pageNumber.ToString(),
            ["pageSize"] = pageSize.ToString(),
        };

        // convert the query dictionary to an url encoded string
        var dictFormUrlEncoded = new FormUrlEncodedContent(query);
        var queryString = await dictFormUrlEncoded.ReadAsStringAsync();

        string json = await _http.GetStringAsync($"api/accounts?{queryString}");

        List = JsonSerializer.Deserialize<PaginatedList<TradingAccount>>(json, _jsonOptions);
    }

    public async Task DeleteTradingAccount(int id)
    {
        await _http.DeleteAsync($"api/accounts/{id}");
    }

    public async Task SetStatus(int id, TradingAccountStatus status)
    {
        var request = new SetTradingAccountStateCommand { Id = id, Status = status };
        await _http.PutAsJsonAsync("api/accounts/set-status", request);
    }

    public async Task<ServiceResponse<int>> AddTradingAccount(CreateTradingAccountCommand command)
    {
        var result = await _http.PostAsJsonAsync($"api/accounts/", command);

        var response = new ServiceResponse<int>() 
        {
            Success = result.StatusCode == HttpStatusCode.OK,
        };

        // deserialize fluent validation errors if request failed
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
