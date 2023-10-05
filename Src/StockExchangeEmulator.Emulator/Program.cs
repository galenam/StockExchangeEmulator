using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using StockExchangeEmulator.Emulator;
using StockExchangeEmulator.Emulator.Storage;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((_, configuration) =>
    {
        configuration.Sources.Clear();
        configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        configuration.Build();
    })
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;
        services.AddSingleton<StockExchangePrice>();
        services.AddSingleton<ICreator, Creator>();
        services.AddSingleton<IStorage, InMemoryStorage>();

        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });

        services.AddOptions<Settings>()
                    .Bind(configuration.GetSection(nameof(Settings)));
    })
    .UseSerilog((context, _, loggerConfiguration) => loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext())
    .Build();

using IServiceScope serviceScope = host.Services.CreateScope();
var provider = serviceScope.ServiceProvider;
provider.GetRequiredService<StockExchangePrice>();
Console.ReadLine();
