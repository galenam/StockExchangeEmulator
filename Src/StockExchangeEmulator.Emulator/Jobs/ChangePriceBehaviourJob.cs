using Microsoft.Extensions.Logging;
using Quartz;
using StockExchangeEmulator.Domain.Enum;
using StockExchangeEmulator.Emulator.Storage.PriceChanging;

namespace StockExchangeEmulator.Emulator.Jobs;

internal sealed class ChangePriceBehaviourJob : IJob
{
    private readonly int _maxValue;
    private readonly Random _random = new ();

    private readonly ILogger<ChangePriceBehaviourJob> _logger;
    private readonly IPriceChangingStorage _priceChangingStorage;

    public ChangePriceBehaviourJob(
        ILogger<ChangePriceBehaviourJob> logger,
        IPriceChangingStorage priceChangingStorage)
    {
        _maxValue = (int)Enum.GetValues(typeof(PriceChangingEnum)).Cast<PriceChangingEnum>().Max() + 1;

        _logger = logger;
        _priceChangingStorage = priceChangingStorage;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var priceChanging = (PriceChangingEnum)_random.Next(_maxValue);

        await _priceChangingStorage.Save(priceChanging);

        _logger.LogInformation("{ChangePriceBehaviourName} _priceChanging={PriceChanging}",
            nameof(ChangePriceBehaviourJob),
            priceChanging);
    }
}
