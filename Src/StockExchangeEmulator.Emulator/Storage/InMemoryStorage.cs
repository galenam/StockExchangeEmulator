using Microsoft.Extensions.Options;

namespace StockExchangeEmulator.Emulator.Storage;

internal sealed class InMemoryStorage: IStorage
{
    private int _sum;
    private int _count;

    public InMemoryStorage(IOptions<Settings> options)
    {
        _count = options.Value.DefaultCount;
        _sum = options.Value.DefaultSum;
    }

    public Task<int> GetSumAsync() => Task.FromResult(_sum);

    public Task<int> GetCountAsync() => Task.FromResult(_count);

    public Task SaveSumAsync(int sum)
    {
        _sum = sum;
        return Task.CompletedTask;
    }

    public Task SaveCountAsync(int count)
    {
        _count = count;
        return Task.CompletedTask;
    }
}
