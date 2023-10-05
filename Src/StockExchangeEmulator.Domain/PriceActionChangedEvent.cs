using MediatR;
using Action = StockExchangeEmulator.Domain.Enum.Action;

namespace StockExchangeEmulator.Domain;

public sealed record PriceActionChangedEvent(
    int Price,
    Action Action) : INotification;
