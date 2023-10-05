using MediatR;
using Microsoft.Extensions.Logging;
using StockExchangeEmulator.Domain;

namespace StockExchangeEmulator.Emulator;

public class TradingStrategyHandler : INotificationHandler<PriceChangedEvent>
{
    private readonly IStrategy _strategy;
    private readonly ILogger<TradingStrategyHandler> _logger;
    private readonly IMediator _mediator;

    public TradingStrategyHandler(
        ICreator creator,
        ILogger<TradingStrategyHandler> logger,
        IMediator mediator)
    {
        _strategy = creator.Create();
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Handle(PriceChangedEvent priceChangedEvent, CancellationToken cancellationToken)
    {
        var action = _strategy.GetAction(priceChangedEvent.Price, priceChangedEvent.OldPrice);
        _logger.LogInformation("Price action was changed, new value {Action}", action);
        await _mediator.Publish(new PriceActionChangedEvent(priceChangedEvent.Price, action), cancellationToken);
    }
}
