using Microsoft.Extensions.Options;
using StockExchangeEmulator.Domain.Enum;

namespace StockExchangeEmulator.Emulator;

public interface ICreator
{
    IStrategy Create();
}

public class Creator : ICreator
{
    private readonly StrategyType _strategyType;
    public Creator(IOptions<Settings> options)
    {
        _strategyType = options.Value.StrategyType.GetEnumValueByDisplayName<StrategyType>();
    }

    public IStrategy Create() => _strategyType switch
    {
        StrategyType.Simple => new SimpleStrategy(),
        _ => new EmptyStrategy()
    };
}
