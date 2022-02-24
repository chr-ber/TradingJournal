using System.Net.Http.Json;
using TradingJournal.Application.Common.Models;
using TradingJournal.Application.Common.Interfaces;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace TradingJournal.Infrastructure.Client.Authentication;

public class CustomAuthenticationService : ICustomAuthenticationService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorageService;
    private readonly AuthenticationStateProvider _authenticationStateProvider;


    public CustomAuthenticationService(
        HttpClient http,
        ILocalStorageService localStorageService,
        AuthenticationStateProvider authenticationStateProvider)
    {
        _http = http;
        _localStorageService = localStorageService;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<ServiceResponse<string>> Login(UserLogin request)
    {
        var result = await _http.PostAsJsonAsync("api/authentication/login", request);

        var response = await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();

        if (response.Success)
        {
            await _localStorageService.SetItemAsync<string>("authToken", response.Data);
            await _authenticationStateProvider.GetAuthenticationStateAsync();
        }

        return response;
    }

    public async Task<ServiceResponse<int>> Register(UserRegistration request)
    {
        var result = await _http.PostAsJsonAsync("api/authentication/register", request);

        return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
    }

    public async Task Logout()
    {
        await _localStorageService.RemoveItemAsync("authToken");
        await _authenticationStateProvider.GetAuthenticationStateAsync();
    }
}
