using Microsoft.Extensions.Options;

public class StockExchangePrice : IObservable<int>
{
    IList<IObserver<int>> _observers;
    Settings _settings;
    static Timer _timerChangeBehaviour;
    static Timer _timerChangePrice;  
    Random _random = new Random();
    int _maxValue = (int)Enum.GetValues(typeof(PriceChangingEnum)).Cast<PriceChangingEnum>().Max() + 1;
    int _price;
    PriceChangingEnum _priceChanging;

    public StockExchangePrice(IOptions<Settings> options)
    {
        _settings = options.Value;
        _timerChangeBehaviour = new Timer(new TimerCallback(ChangePriceBehaviour), _priceChanging, 0, _settings.ChangeBehaviorInterval * 1000);
        _timerChangePrice = new Timer(new TimerCallback(ChangePrice), _price, 0, _settings.ChangePriceInterval * 1000);
        _observers = new List<IObserver<int>>();
    }

    private void ChangePrice(object? state)
    {
        switch (_priceChanging)
        {
            case PriceChangingEnum.Higher:
                _price++;
                break;
            case PriceChangingEnum.Lower:
                _price--;
                break;
        }
    }

    private void ChangePriceBehaviour(object? priceChanging)
    {
        _priceChanging = (PriceChangingEnum)_random.Next(_maxValue);
    }


    public IDisposable Subscribe(IObserver<int> observer)
    {
        if (!_observers.Contains(observer)){
            _observers.Add(observer);
            foreach (var item in _observers)
            {
                item.OnNext(_price);
            }
        }
        return null;
    }
}