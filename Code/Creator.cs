using Microsoft.Extensions.Options;

public interface ICreator
{
    IStrategy Create();
}

public class Creator : ICreator
{
    StrategyType _strategyType;
    IOptions<Settings> _options;
    public Creator(IOptions<Settings> options)
    {
        _strategyType = options.Value.StrategyType.GetEnumValueByDisplayName<StrategyType>();
        _options = options;
    }
    public IStrategy Create()
    {
        switch (_strategyType)
        {
            case StrategyType.Simple:
                return new SimpleStategy(_options);
            default:
                return new EmptyStategy();
        }
    }
}