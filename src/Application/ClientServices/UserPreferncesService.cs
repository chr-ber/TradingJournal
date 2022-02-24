using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Application.Common.Models;
using Blazored.LocalStorage;

namespace TradingJournal.Application.ClientServices;

class UserPreferncesService : IUserPreferncesService
{
    private readonly ILocalStorageService _localStorage;
    private const string _key = "userPreferences";

    public UserPreferncesService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task SaveUserPreferences(UserPreferences userPreferences)
    {
        await _localStorage.SetItemAsync(_key, userPreferences);
    }

    public async Task<UserPreferences> LoadUserPreferences()
    {
        return await _localStorage.GetItemAsync<UserPreferences>(_key);
    }
}