using MediatR;

namespace StockExchangeEmulator.Domain;

public sealed record PriceChangedEvent(int Price, int OldPrice)
    : INotification;
