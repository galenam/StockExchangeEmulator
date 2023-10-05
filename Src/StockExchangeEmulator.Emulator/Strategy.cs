using Microsoft.Extensions.Options;
using Action = StockExchangeEmulator.Domain.Enum.Action;

namespace StockExchangeEmulator.Emulator;

public interface IStrategy
{
    Action GetAction(int price, int oldPrice);
}

public class EmptyStrategy : IStrategy
{
    public Action GetAction(int price, int oldPrice) => Action.Hold;
}

public class SimpleStrategy : IStrategy
{
    public Action GetAction(int price, int oldPrice)
    {
        if (price > oldPrice)
        {
            return Action.Sell;
        }
        return price < oldPrice ? Action.Buy : Action.Hold;
    }
}
