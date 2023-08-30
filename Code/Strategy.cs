using Microsoft.Extensions.Options;

public interface IStrategy
{
    Action GetAction(int value);
}

public class EmptyStategy : IStrategy
{
    public Action GetAction(int value) => Action.Hold;
}

public class SimpleStategy : IStrategy
{
    int _lastPrice;

    public SimpleStategy(IOptions<Settings> _options)
    {
        _lastPrice = _options.Value.InitialPrice;
    }
    public Action GetAction(int value)
    {
        var last = _lastPrice;
        _lastPrice = value;
        if (last > value)
        {
            return Action.Buy;
        }
        else if (last < value)
        {
            return Action.Sell;
        }
        return Action.Hold;
    }
}

