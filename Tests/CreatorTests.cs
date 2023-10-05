using Microsoft.Extensions.Options;
using StockExchangeEmulator.Domain.Enum;
using StockExchangeEmulator.Emulator;

namespace StockExchangeEmulator.Tests;

public class CreatorTests
{
    [TestCase("Simple", typeof(SimpleStrategy))]
    [TestCase("Empty", typeof(EmptyStrategy))]
    public void CreatorCreateShouldReturnRightType(string strategyType, Type type)
    {
        var options = Options.Create(new Settings { StrategyType = strategyType });
        var creator = new Creator(options);
        var strategy = creator.Create();

        Assert.That(type, Is.EqualTo(strategy.GetType()));
    }

    [TestCase("Simple", StrategyType.Simple)]
    [TestCase("Empty", StrategyType.Empty)]
    public void HelperGetEnumValueByDisplayName(string name, StrategyType sType)
    {
        var strategyType = Helper.GetEnumValueByDisplayName<StrategyType>(name);
        Assert.That(sType, Is.EqualTo(strategyType));
    }
}
