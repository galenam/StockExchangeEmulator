using StockExchangeEmulator.Domain.Enum;

namespace StockExchangeEmulator.Emulator.Storage.PriceChanging;

internal sealed class PriceChangingStorage : IPriceChangingStorage
{
    private PriceChangingEnum _priceChanging = PriceChangingEnum.Stable;

    public Task Save(PriceChangingEnum priceChanging)
    {
        _priceChanging = priceChanging;
        return Task.CompletedTask;
    }

    public Task<PriceChangingEnum> Get() => Task.FromResult(_priceChanging);
}
