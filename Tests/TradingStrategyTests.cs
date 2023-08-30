using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace tests;

public class TradingStrategyTests
{
    [Test]
    public void TradingStrategyShouldCallOnNextOnce()
    {
        var stockExchangePrice = new Mock<IStockExchangePrice>();
        var creator = new Mock<ICreator>();
        creator.Setup(c => c.Create()).Returns(new EmptyStategy());
        var logger = new Mock<ILogger<TradingStrategy>>();
        var tradingStrategy = new TradingStrategy(stockExchangePrice.Object, creator.Object, logger.Object);
        var observer = Mock.Of<IObserver<Action>>();
        tradingStrategy.Subscribe(observer);
        tradingStrategy.OnNext(0);
        Mock.Get(observer).Verify(x => x.OnNext(Action.Hold), Times.Once());
    }
}