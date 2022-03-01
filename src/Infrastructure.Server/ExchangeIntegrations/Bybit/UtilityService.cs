using TradingJournal.Infrastructure.Server.ExchangeIntegrations.Bybit.Models;
using TradingJournal.Application.Common.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace TradingJournal.Infrastructure.Server.ExchangeIntegrations.Bybit;

public class UtilityService : IUtilityService
{
    public async Task<bool> IsReadOnlyAPICredentials(string key, string secret)
    {
        var timestamp = GetExpirationInUnixMilliseconds();

        string signature = CreateSignature(secret, $"api_key={key}&timestamp={timestamp}");

        using (var client = new HttpClient())
        {
            var content = await client.GetStringAsync($"https://api-testnet.bybit.com/v2/private/account/api-key?timestamp={ timestamp }&api_key={ key }&sign={ signature  }");

            if (string.IsNullOrEmpty(content))
                throw new Exception("Unable to get response from api-key endpoint.");

            var response = JsonSerializer.Deserialize<WebSocketResponse<List<ApiKeyInfo>>>(content);

            if(response.result is null)
                throw new Exception("Received unexpected response from api-key endpoint, unable to parse read-only flag.");

            return response.result[0].read_only;
        }
    }

    internal static string CreateSignature(string secret, string message)
    {
        byte[] signatureBytes = Hmacsha256(Encoding.UTF8.GetBytes(secret), Encoding.UTF8.GetBytes(message));

        return ByteArrayToString(signatureBytes);
    }

    private static byte[] Hmacsha256(byte[] keyByte, byte[] messageBytes)
    {
        using (HMACSHA256 hash = new(keyByte))
        {
            return hash.ComputeHash(messageBytes);
        }
    }

    private static string ByteArrayToString(byte[] byteArray)
    {
        StringBuilder hex = new(byteArray.Length * 2);

        foreach (byte b in byteArray)
        {
            hex.AppendFormat("{0:x2}", b);
        }
        return hex.ToString();
    }

    internal static long GetExpirationInUnixMilliseconds() => DateTimeOffset.Now.ToUnixTimeMilliseconds();

}
