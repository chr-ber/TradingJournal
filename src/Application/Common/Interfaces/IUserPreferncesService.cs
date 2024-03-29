﻿namespace TradingJournal.Application.Common.Interfaces;

public interface IUserPreferncesService
{
   Task<UserPreferences> LoadUserPreferences();
   Task SaveUserPreferences(UserPreferences userPreferences);
}
