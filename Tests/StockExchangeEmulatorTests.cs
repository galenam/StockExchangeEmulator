using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

public class StockExchangeEmulatorTests
{
    [Test]
    public void StockExchangeEmulator()
    {
        var tradingStrategy = new Mock<ITradingStrategy>();
        var stockExchangePrice = new Mock<IStockExchangePrice>();
        var logger = new Mock<ILogger<StockExchangeEmulator>>();
        var options = Options.Create<Settings>(new Settings { StrategyType = "Simple", DefaultCount = 200, DefaultSum = 1000 });
        var stockExchangeEmulator = new StockExchangeEmulator(options, tradingStrategy.Object, stockExchangePrice.Object, logger.Object);
        var observer = Mock.Of<IObserver<ActionSum>>();
        stockExchangeEmulator.Subscribe(observer);
        stockExchangeEmulator.OnNext(Action.Buy);
        Mock.Get(observer).Verify(x => x.OnNext(It.IsAny<ActionSum>()), Times.Once());
    }
}