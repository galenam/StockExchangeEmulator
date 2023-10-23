using Microsoft.Extensions.Options;

namespace StockExchangeEmulator.Emulator.Storage.PriceStorage;

internal sealed class PriceStorage : IPriceStorage
{
    private int _price;

    public PriceStorage(IOptions<Settings> options)
    {
        _price = options.Value.InitialPrice;
    }

    public Task Save(int price)
    {
        _price = price;
        return Task.CompletedTask;
    }

    public Task<int> Get() => Task.FromResult(_price);
}
