using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StockExchangeEmulator.Domain;
using StockExchangeEmulator.Emulator.Storage;
using Action = StockExchangeEmulator.Domain.Enum.Action;

namespace StockExchangeEmulator.Emulator;
public class StockExchangeEmulatorHandler : INotificationHandler<PriceActionChangedEvent>
{
    private readonly Settings _settings;
    private readonly ILogger<StockExchangeEmulatorHandler> _logger;
    private readonly IStorage _storage;

    public StockExchangeEmulatorHandler(
        IOptions<Settings> options,
        ILogger<StockExchangeEmulatorHandler> logger,
        IStorage storage)
    {
        _settings = options.Value;
        _logger = logger;
        _storage = storage;
    }

    public async Task Handle(PriceActionChangedEvent notification, CancellationToken cancellationToken)
    {
        var count = await _storage.GetCountAsync();
        var initialCount = count;
        var sum = await _storage.GetSumAsync();
        var initialSum = sum;
        switch (notification.Action)
        {
            case Action.Buy:
                count += _settings.DefaultBuy;
                sum += _settings.DefaultBuy * notification.Price;
                break;
            case Action.Sell:
                var sell = count < _settings.DefaultSell ? count : _settings.DefaultSell;
                if (sell == 0) { return; }
                count -= sell;
                sum -= sell * notification.Price;
                if (sum < 0)
                {
                    return;
                }

                break;
        }

        if (count != initialCount)
        {
            await _storage.SaveCountAsync(count);
            _logger.LogInformation("Count changed oldValue={StorageCount}, value={Count}", initialCount, count);
        }

        if (sum != initialSum)
        {
            await _storage.SaveSumAsync(sum);
            _logger.LogInformation("Sum changed oldValue={StorageSum}, value={Sum}", initialSum, sum);
        }
    }
}
