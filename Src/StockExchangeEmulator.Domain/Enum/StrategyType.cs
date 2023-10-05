using System.ComponentModel.DataAnnotations;

namespace StockExchangeEmulator.Domain.Enum;

public enum StrategyType
{
    [Display(Name = "Simple")]
    Simple,
    [Display(Name = "Empty")]
    Empty
}
