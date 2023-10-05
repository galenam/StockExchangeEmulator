using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using StockExchangeEmulator.Domain;
using StockExchangeEmulator.Emulator;
using StockExchangeEmulator.Emulator.Storage;
using Action = StockExchangeEmulator.Domain.Enum.Action;

namespace StockExchangeEmulator.Tests;

public class StockExchangeEmulatorTests
{
    private const int DEFAULT_BUY = 10;
    private const int DEFAULT_SELL = 5;
    private const int COUNT = 18;
    private const int SUM = 70;

    private readonly StockExchangeEmulatorHandler _handler;
    private readonly Mock<IStorage> _storageMock = new ();

    public StockExchangeEmulatorTests()
    {
        var settings = new Settings
        {
            DefaultBuy = DEFAULT_BUY,
            DefaultSell = DEFAULT_SELL
        };
        var optionsMock = new Mock<IOptions<Settings>>();
        optionsMock
            .Setup(o => o.Value)
            .Returns(settings);

        var loggerMock = new Mock<ILogger<StockExchangeEmulatorHandler>>();
        _storageMock = new Mock<IStorage>();
        _storageMock
            .Setup(s => s.GetCountAsync())
            .ReturnsAsync(COUNT);
        _storageMock
            .Setup(s => s.GetSumAsync())
            .ReturnsAsync(SUM);

        _handler = new StockExchangeEmulatorHandler(
            optionsMock.Object,
            loggerMock.Object,
            _storageMock.Object);
    }

    [TestCase(5, Action.Buy, 28, 120)]
    [TestCase(7, Action.Sell, 13, 35)]
    public async Task Handle_CorrectData_ShouldUpdateStorage(int price, Action action, int resultCount, int resultSum)
    {
        _storageMock.Invocations.Clear();
        var request = new PriceActionChangedEvent(price, action);
        await _handler.Handle(request, CancellationToken.None);

        _storageMock
            .Verify(s => s.GetCountAsync(), Times.Once);
        _storageMock
            .Verify(s => s.GetSumAsync(), Times.Once);
        _storageMock
            .Verify(s => s.SaveCountAsync(resultCount), Times.Once);
        _storageMock
            .Verify(s => s.SaveSumAsync(resultSum), Times.Once);
    }

    [TestCase(3000, Action.Sell)]
    public async Task Handle_SumNegative_ShouldNotUpdateStorage(int price, Action action)
    {
        _storageMock.Invocations.Clear();
        var request = new PriceActionChangedEvent(price, action);
        await _handler.Handle(request, CancellationToken.None);

        _storageMock
            .Verify(s => s.GetCountAsync(), Times.Once);
        _storageMock
            .Verify(s => s.GetSumAsync(), Times.Once);
        _storageMock
            .Verify(s => s.SaveCountAsync(It.IsAny<int>()), Times.Never);
        _storageMock
            .Verify(s => s.SaveSumAsync(It.IsAny<int>()), Times.Never);
    }
}
