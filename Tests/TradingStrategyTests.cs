using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using StockExchangeEmulator.Domain;
using StockExchangeEmulator.Emulator;
using Action = StockExchangeEmulator.Domain.Enum.Action;

namespace StockExchangeEmulator.Tests;

public class TradingStrategyTests
{
    [Test]
    public async Task TradingStrategyShouldCallOnNextOnce()
    {
        const int PRICE = 5;
        const int OLD_PRICE = 4;
        const Action ACTION = Action.Buy;

        var strategy = new Mock<IStrategy>();
        strategy
            .Setup(s => s.GetAction(
                It.IsAny<int>(),
                It.IsAny<int>()))
            .Returns(ACTION);

        var logger = new Mock<ILogger<TradingStrategyHandler>>();

        var creator = new Mock<ICreator>();
        creator
            .Setup(c => c.Create())
            .Returns(strategy.Object);

        var mediator = new Mock<IMediator>();

        var tradingStrategyHandler = new TradingStrategyHandler(
            creator.Object,
            logger.Object,
            mediator.Object);

        var @event = new PriceChangedEvent(PRICE, OLD_PRICE);
        await tradingStrategyHandler.Handle(@event, CancellationToken.None);
        mediator
            .Verify(m => m.Publish(It.Is<PriceActionChangedEvent>(
                e => e.Action == ACTION && e.Price == PRICE),
                    It.IsAny<CancellationToken>()),
                Times.Once());
    }
}
