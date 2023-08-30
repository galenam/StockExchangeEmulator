using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
public interface IStockExchangeEmulator : IObserver<int>, IObserver<Action>, IObservable<ActionSum> { }
public class StockExchangeEmulator : IStockExchangeEmulator
{
    int _count;
    int _summ;
    int _price;
    Settings _settings;
    ITradingStrategy _tradingStrategy;
    IStockExchangePrice _stockExchangePrice;
    ILogger<StockExchangeEmulator> _logger;

    IList<IObserver<ActionSum>> _observers = new List<IObserver<ActionSum>>();
    public StockExchangeEmulator(IOptions<Settings> options, ITradingStrategy tradingStrategy, IStockExchangePrice stockExchangePrice,
        ILogger<StockExchangeEmulator> logger)
    {
        _count = options.Value.DefaultCount;
        _summ = options.Value.DefaultSum;
        _settings = options.Value;
        _tradingStrategy = tradingStrategy;
        _tradingStrategy.Subscribe(this);
        _stockExchangePrice = stockExchangePrice;
        _stockExchangePrice.Subscribe(this);
        _logger = logger;
    }

    public void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public void OnNext(Action value)
    {
        switch (value)
        {
            case Action.Buy:
                _count += _settings.DefaultBuy;
                _summ += _settings.DefaultBuy * _price;
                break;
            case Action.Sell:
                var sell = _count < _settings.DefaultSell ? _count : _settings.DefaultSell;
                if (sell == 0) { return; }
                _count -= sell;
                _summ -= sell * _price;
                break;
            default:
                break;
        }
        foreach (var o in _observers)
        {
            _logger.LogInformation($"{nameof(StockExchangeEmulator)} call {nameof(OnNext)} Action={value} Sum={_summ}");
            o.OnNext(new ActionSum { Action = value, Sum = _summ });
        }
    }

    public void OnNext(int value)
    {
        _logger.LogInformation($"{nameof(StockExchangeEmulator)} {nameof(OnNext)} inside value={value}");
        _price = value;
    }

    public IDisposable Subscribe(IObserver<ActionSum> observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
        return Disposable.Empty;
    }
}