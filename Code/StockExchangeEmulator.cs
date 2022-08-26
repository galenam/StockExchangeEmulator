using Microsoft.Extensions.Options;

public class StockExchangeEmulator : IObserver<int>, IObserver<Action>
{
    int _count;
    int _summ;
    Settings _settings;
    public StockExchangeEmulator(IOptions<Settings> options)
    {
        _count = options.Value.DefaultCount;
        _summ = options.Value.DefaultSum;
        _settings = options.Value;
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
        throw new NotImplementedException();
    }

    public void OnNext(int value)
    {
        throw new NotImplementedException();
    }
}