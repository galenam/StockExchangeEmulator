using StockExchangeEmulator.Domain.Enum;

namespace StockExchangeEmulator.Emulator.Storage.PriceChanging;

public interface IPriceChangingStorage
{
    public Task Save(PriceChangingEnum priceChanging);

    public Task<PriceChangingEnum> Get();
}
