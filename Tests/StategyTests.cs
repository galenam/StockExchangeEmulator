using Microsoft.Extensions.Options;
using StockExchangeEmulator.Emulator;
using Action = StockExchangeEmulator.Domain.Enum.Action;

namespace StockExchangeEmulator.Tests;

public class StategyTests
{
    [TestCase(1, 1)]
    [TestCase(0, 0)]
    [TestCase(-1, -1)]
    public void EmptyStrategyShouldHold(int price, int oldPrice)
    {
        var action = new EmptyStrategy().GetAction(price, oldPrice);
        Assert.That(action, Is.EqualTo(Action.Hold));
    }

    [TestCase(11, 10, Action.Sell)]
    [TestCase(10, 10, Action.Hold)]
    [TestCase(9, 10, Action.Buy)]
    public void SimpleStrategyShouldReturnAction(int price, int oldPrice, Action rightAction)
    {
        var action = new SimpleStrategy().GetAction(price, oldPrice);
        Assert.That(rightAction, Is.EqualTo(action));
    }
}
