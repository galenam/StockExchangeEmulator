using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
public interface ITradingStrategy : IObserver<int>, IObservable<Action> { }

public class TradingStrategy : ITradingStrategy
{
    IStockExchangePrice _stockExchangePrice;
    IStrategy _strategy;
    IList<IObserver<Action>> _observers = new List<IObserver<Action>>();
    ILogger<TradingStrategy> _logger;

    public TradingStrategy(IStockExchangePrice stockExchangePrice, ICreator creator, ILogger<TradingStrategy> logger)
    {
        _stockExchangePrice = stockExchangePrice;
        _stockExchangePrice.Subscribe(this);
        _strategy = creator.Create();
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

    public void OnNext(int value)
    {
        var action = _strategy.GetAction(value);
        foreach (var o in _observers)
        {
            _logger.LogInformation($"{nameof(TradingStrategy)} OnNext action={action}");
            o.OnNext(action);
        }
    }

    public IDisposable Subscribe(IObserver<Action> observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
        return Disposable.Empty;
    }
}