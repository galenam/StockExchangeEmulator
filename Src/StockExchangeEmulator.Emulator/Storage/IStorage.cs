namespace StockExchangeEmulator.Emulator.Storage;

public interface IStorage
{
    Task<int> GetSumAsync();
    Task<int> GetCountAsync();
    Task SaveSumAsync(int sum);
    Task SaveCountAsync(int count);
}
