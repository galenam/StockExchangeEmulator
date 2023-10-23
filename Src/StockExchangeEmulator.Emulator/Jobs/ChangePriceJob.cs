using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using StockExchangeEmulator.Domain;
using StockExchangeEmulator.Domain.Enum;
using StockExchangeEmulator.Emulator.Storage.PriceChanging;
using StockExchangeEmulator.Emulator.Storage.PriceStorage;

namespace StockExchangeEmulator.Emulator.Jobs;

internal sealed class ChangePriceJob : IJob
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IPriceChangingStorage _priceChangingStorage;
    private readonly IPriceStorage _priceStorage;

    public ChangePriceJob(
        IPriceChangingStorage priceChangingStorage,
        IServiceProvider serviceProvider,
        IPriceStorage priceStorage)
    {
        _priceChangingStorage = priceChangingStorage;
        _serviceProvider = serviceProvider;
        _priceStorage = priceStorage;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var oldPrice = await _priceStorage.Get();
        var price = oldPrice;
        var priceChanging = await _priceChangingStorage.Get();
        switch (priceChanging)
        {
            case PriceChangingEnum.Higher:
                price++;
                break;
            case PriceChangingEnum.Lower:
                if (price > 1)
                {
                    price--;
                }
                break;
        }

        if (oldPrice == price) return;

        var mediatr = _serviceProvider.GetRequiredService<IMediator>();
        await mediatr.Publish(new PriceChangedEvent(price, oldPrice));
    }
}
