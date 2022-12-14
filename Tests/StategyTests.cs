using Microsoft.Extensions.Options;

namespace tests;

public class StategyTests
{
    [TestCase(1)]
    [TestCase(0)]
    [TestCase(-1)]
    public void EmptyStrategyShouldHold(int value)
    {
        var action = (new EmptyStategy()).GetAction(value);
        Assert.AreEqual(action, Action.Hold);
    }

    [TestCase(11, Action.Sell)]
    [TestCase(10, Action.Hold)]
    [TestCase(9, Action.Buy)]
    public void SimpleStrategyShouldReturnAction(int value, Action rightAction)
    {
        var options = Options.Create<Settings>(new Settings { InitialPrice = 10 });
        var action = (new SimpleStategy(options)).GetAction(value);
        Assert.AreEqual(rightAction, action);
    }
}