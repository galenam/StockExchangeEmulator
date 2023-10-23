namespace StockExchangeEmulator.Emulator.Storage.PriceStorage;

public interface IPriceStorage
{
    public Task Save(int price);

    public Task<int> Get();
}
