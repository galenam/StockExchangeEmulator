using Microsoft.Extensions.Options;

public interface ICreator
{
    IStrategy Create();
}

public class Creator : ICreator
{
    StrategyType _strategyType;
    public Creator(IOptions<Settings> options)
    {
        _strategyType = options.Value.StrategyType.GetEnumValueByDisplayName<StrategyType>();
    }
    public IStrategy Create()
    {
        switch (_strategyType)
        {
            case StrategyType.Simple:
                return new SimpleStategy();
            default:
                return new EmptyStategy();
        }
    }
}