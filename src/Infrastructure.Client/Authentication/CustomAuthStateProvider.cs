using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using System.Security.Claims;
using System.Text.Json;

namespace TradingJournal.Infrastructure.Client.Authentication
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _httpClient;

        public CustomAuthStateProvider(
            ILocalStorageService localStorageService,
            HttpClient httpClient
            )
        {
            _localStorageService = localStorageService;
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string authToken = await _localStorageService.GetItemAsStringAsync("authToken");

            ClaimsIdentity identity = new();
            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(authToken))
            {
                try
                {
                    identity = new(ParseClaimsFromJwt(authToken), "jwt");
                    _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", authToken.Trim('"'));
                }
                catch
                {
                    await _localStorageService.RemoveItemAsync("authToken");
                    identity = new ClaimsIdentity();
                }
            }

            ClaimsPrincipal user = new(identity);
            AuthenticationState state = new(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;

        }
        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            base64 += (base64.Length % 4) switch
            {
                2 => "==",
                3 => "=",
                _ => String.Empty,
            };

            return Convert.FromBase64String(base64);
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            string payload = jwt.Split('.')[1];
            byte[] jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if(keyValuePairs != null)
                return keyValuePairs.Select(x => new Claim(x.Key, x.Value.ToString()));

            // return empty list if unable to parse claims
            return new List<Claim>();
        }
    }
}
