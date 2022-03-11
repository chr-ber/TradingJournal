using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Domain.Entities;
using System.Collections.Concurrent;
using MediatR;

namespace TradingJournal.Application.Entities.Symbols.Commands.CreateSymbolIfMissing;

public class CreateSymbolIfMissingCommand : IRequest<int>
{
    public string Name { get; set; }

}

public class CreateSymbolIfMissingCommandHandler : IRequestHandler<CreateSymbolIfMissingCommand, int>
{
    private readonly IApplicationDbContext _context;

    private static ConcurrentDictionary<string, int> _symbolToDatabaseIdMap;

    public CreateSymbolIfMissingCommandHandler(IApplicationDbContext context)
    {
        _context = context;

        // cache all symbols on startup
        if(_symbolToDatabaseIdMap == null)
        {
            _symbolToDatabaseIdMap = new();
            var symbols = _context.Symbols.ToList();

            foreach (var symbol in symbols)
            {
                _symbolToDatabaseIdMap.TryAdd(symbol.Name, symbol.Id);
            }
        }
    }

    public async Task<int> Handle(CreateSymbolIfMissingCommand request, CancellationToken cancellationToken)
    {
        int id;

        // see if symbol is in cache
        if (_symbolToDatabaseIdMap.ContainsKey(request.Name))
        {
            id = _symbolToDatabaseIdMap[request.Name];
        }
        else
        {
            // try to get symbol from db
            var symbol = _context.Symbols.FirstOrDefault(x => x.Name == request.Name);

            // if symbol cant be found create new one
            if (symbol == null)
            {
                symbol = new Symbol()
                {
                    Name = request.Name,
                };

                _context.Symbols.Add(symbol);
                await _context.SaveChangesAsync(cancellationToken);
            }
            // add symbol to cache
            _symbolToDatabaseIdMap.TryAdd(symbol.Name,symbol.Id);

            id = symbol.Id;
        }
        return id;
    }
}
