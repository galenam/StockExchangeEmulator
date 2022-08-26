using System.Reactive.Disposables;

public class TradingStrategy : IObserver<int>, IObservable<Action>
{
    StockExchangePrice _stockExchangePrice;
    IStrategy _strategy;
    IList<IObserver<Action>> _observers = new List<IObserver<Action>>();

    public TradingStrategy(StockExchangePrice stockExchangePrice, ICreator creator)
    {
        _stockExchangePrice = stockExchangePrice;
        _stockExchangePrice.Subscribe(this);
        _strategy = creator.Create();
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