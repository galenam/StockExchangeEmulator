using Action = StockExchangeEmulator.Domain.Enum.Action;

namespace StockExchangeEmulator.Emulator;

public class ActionSum
{
    public int Sum { get; set; }
    public Action Action { get; set; }
    public override string ToString() => $"Action={Action} Sum={Sum}";
}
