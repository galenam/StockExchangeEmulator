using System.ComponentModel.DataAnnotations;

public enum PriceChangingEnum
{
    Higher,
    Lower,
    Stable
}

public enum Action
{
    Sell,
    Buy,
    Hold
}

public enum StrategyType
{
    [Display(Name = "Simple")]
    Simple,
    [Display(Name = "Empty")]
    Empty
}