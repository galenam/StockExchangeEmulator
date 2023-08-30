using Microsoft.Extensions.Options;

namespace tests;

public class CreatorTests
{
    [TestCase("Simple", typeof(SimpleStategy))]
    [TestCase("Empty", typeof(EmptyStategy))]
    public void CreatorCreateShouldReturnRightType(string strategyType, Type type)
    {
        var options = Options.Create<Settings>(new Settings { StrategyType = strategyType });
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
