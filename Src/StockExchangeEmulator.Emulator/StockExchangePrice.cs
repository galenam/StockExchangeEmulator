using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StockExchangeEmulator.Domain;
using StockExchangeEmulator.Domain.Enum;

namespace StockExchangeEmulator.Emulator;

public class StockExchangePrice : IDisposable
{
    private readonly Timer _timerChangeBehaviour;
    private readonly Timer _timerChangePrice;
    private readonly Random _random = new ();
    private readonly int _maxValue;
    private readonly ILogger<StockExchangePrice> _logger;
    private readonly IServiceProvider _serviceProvider;

    private int _price;
    private PriceChangingEnum _priceChanging;

    public StockExchangePrice(
        IOptions<Settings> options,
        ILogger<StockExchangePrice> logger,
        IServiceProvider serviceProvider)
    {
        _maxValue = (int)Enum.GetValues(typeof(PriceChangingEnum)).Cast<PriceChangingEnum>().Max() + 1;

        var settings = options.Value;
        _price = settings.InitialPrice;

        _timerChangeBehaviour = new Timer(ChangePriceBehaviour, _priceChanging, 0, settings.ChangeBehaviorInterval * 1000);
        _timerChangePrice = new Timer(ChangePrice, _price, 0, settings.ChangePriceInterval * 1000);

        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    private async void ChangePrice(object? state)
    {
        var oldPrice = _price;
        switch (_priceChanging)
        {
            case PriceChangingEnum.Higher:
                _price++;
                break;
            case PriceChangingEnum.Lower:
                if (_price > 1)
                {
                    _price--;
                }
                break;
        }

        if (oldPrice == _price) return;

        var mediatr = _serviceProvider.GetRequiredService<IMediator>();
        await mediatr.Publish(new PriceChangedEvent(_price, oldPrice));
    }

    private void ChangePriceBehaviour(object? priceChanging)
    {
        _priceChanging = (PriceChangingEnum)_random.Next(_maxValue);
        _logger.LogInformation("{StockExchangePriceName} {ChangePriceBehaviourName} _priceChanging={PriceChanging}",
            nameof(StockExchangePrice),
            nameof(ChangePriceBehaviour), _priceChanging);
    }

    public void Dispose()
    {
        _timerChangeBehaviour.Dispose();
        _timerChangePrice.Dispose();
    }
}
