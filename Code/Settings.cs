public class Settings
{
    public int InitialPrice { get; set; }
    public int ChangeBehaviorInterval { get; set; }
    public int ChangePriceInterval { get; set; }
    public string StrategyType { get; set; } = String.Empty;
    public int DefaultCount { get; set; }
    public int DefaultBuy { get; set; }
    public int DefaultSell { get; set; }
    public int DefaultSum { get; set; }
}
