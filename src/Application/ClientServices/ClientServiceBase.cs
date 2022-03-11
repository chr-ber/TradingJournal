using System.Text.Json;

namespace TradingJournal.Application.ClientServices;

// base class for all client services
public class ClientServiceBase
{
    protected readonly JsonSerializerOptions _jsonOptions;

    public ClientServiceBase()
    {
        // ignore case sensivity
        // all api controllers provide names starting with lower case but properties start with higher case
        _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
    }
}

