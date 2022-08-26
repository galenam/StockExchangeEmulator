public class Settings
{
    public int InitialPrice { get; set; }
    public int ChangeBehaviorInterval { get; set; }
    public int ChangePriceInterval { get; internal set; }
    public string StrategyType { get; internal set; }
    public int DefaultCount { get; internal set; }
    public int DefaultBuy { get; internal set; }
    public int DefaultSell { get; internal set; }
    public int DefaultSum { get; internal set; }
}