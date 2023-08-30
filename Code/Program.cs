using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, configuration) =>
    {
        configuration.Sources.Clear();
        var env = hostingContext.HostingEnvironment;
        configuration
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        IConfigurationRoot configurationRoot = configuration.Build();
        var options =
            configurationRoot.Get<Settings>();
    })
    .ConfigureServices((context, services) =>
    {
        var configurationRoot = context.Configuration;
        services.AddSingleton<IStockExchangePrice, StockExchangePrice>();
        services.AddSingleton<ICreator, Creator>();
        services.AddSingleton<IStockExchangeEmulator, StockExchangeEmulator>();
        services.AddSingleton<ITradingStrategy, TradingStrategy>();

        services.AddOptions<Settings>()
            .Bind(configurationRoot);
    })
    .UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext())
    .Build();

using IServiceScope serviceScope = host.Services.CreateScope();
var provider = serviceScope.ServiceProvider;
var emulator = provider.GetRequiredService<IStockExchangeEmulator>();
emulator.Subscribe(Console.WriteLine);
Console.ReadLine();
