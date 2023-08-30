using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reactive.Disposables;
using System.Reactive.Linq;
public interface IStockExchangePrice : IObservable<int>
{ }
public class StockExchangePrice : IStockExchangePrice
{
    IList<IObserver<int>> _observers = new List<IObserver<int>>();
    Settings _settings;
    Timer _timerChangeBehaviour;
    Timer _timerChangePrice;
    Random _random = new Random();
    int _maxValue = (int)Enum.GetValues(typeof(PriceChangingEnum)).Cast<PriceChangingEnum>().Max() + 1;
    int _price;
    PriceChangingEnum _priceChanging;
    ILogger<StockExchangeEmulator> _logger;

    public StockExchangePrice(IOptions<Settings> options, ILogger<StockExchangeEmulator> logger)
    {
        _settings = options.Value;
        _price = _settings.InitialPrice;
        _timerChangeBehaviour = new Timer(new TimerCallback(ChangePriceBehaviour), _priceChanging, 0, _settings.ChangeBehaviorInterval * 1000);
        _timerChangePrice = new Timer(new TimerCallback(ChangePrice), _price, 0, _settings.ChangePriceInterval * 1000);
        _logger = logger;
    }

    private void ChangePrice(object? state)
    {
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
        foreach (var o in _observers)
        {
            _logger.LogInformation($"{nameof(StockExchangePrice)} {nameof(ChangePrice)} OnNext int _price={_price}");
            o.OnNext(_price);
        }
    }

    private void ChangePriceBehaviour(object? priceChanging)
    {
        _priceChanging = (PriceChangingEnum)_random.Next(_maxValue);
        _logger.LogInformation($"{nameof(StockExchangePrice)} {nameof(ChangePriceBehaviour)} _priceChanging={_priceChanging}");
    }

    public IDisposable Subscribe(IObserver<int> observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
        return Disposable.Empty;
    }
}