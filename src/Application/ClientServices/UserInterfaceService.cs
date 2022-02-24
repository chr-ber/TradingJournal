using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Application.Common.Models;
using MudBlazor;

namespace TradingJournal.Application.ClientServices;

public class UserInterfaceService
{
    private readonly IUserPreferncesService _userPreferncesService;
    private UserPreferences _userPreferences;

    public bool UseDarkMode { get; private set; }

    public event EventHandler UIChangeOccured;

    public UserInterfaceService(IUserPreferncesService userPreferncesService)
    {
        _userPreferncesService = userPreferncesService;
    }

    public void SetDarkMode(bool value)
    {
            UseDarkMode = value;
    }

    public async Task SetUserPreferencesFromLocalStorage()
    {
        // load user preferences from local storage
        _userPreferences = await _userPreferncesService.LoadUserPreferences();

        if(_userPreferences != null)
        {
            UseDarkMode = _userPreferences.UseDarkMode;
        }
        // if user preferences dont exist in local storage, create it
        else
        {
            _userPreferences = new();
            _userPreferences.UseDarkMode = UseDarkMode = true;
            await _userPreferncesService.SaveUserPreferences(_userPreferences);
        }
        OnUIChangeOccured();
    }

    // toggle dark mode and save preference to local storage
    public async Task ToggleDarkMode()
    {
        _userPreferences.UseDarkMode = UseDarkMode = !UseDarkMode;
        await _userPreferncesService.SaveUserPreferences(_userPreferences);
        OnUIChangeOccured();
    }

    private void OnUIChangeOccured() => UIChangeOccured?.Invoke(this, EventArgs.Empty);
}