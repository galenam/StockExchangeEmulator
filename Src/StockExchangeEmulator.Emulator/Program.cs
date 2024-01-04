using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using StockExchangeEmulator.Emulator;
using StockExchangeEmulator.Emulator.Storage;
using StockExchangeEmulator.Emulator.Storage.PriceChanging;
using Quartz;
using StockExchangeEmulator.Emulator.JobConfig;
using StockExchangeEmulator.Emulator.Jobs;
using StockExchangeEmulator.Emulator.Storage.PriceStorage;
using StockExchangeEmulator.Persistence.Migration;

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
        const string JOB_CONFIGS = "JobConfigs";
        var configuration = context.Configuration;
        services.AddSingleton<ICreator, Creator>();
        services.AddSingleton<IStorage, InMemoryStorage>();
        services.AddSingleton<IPriceChangingStorage, PriceChangingStorage>();
        services.AddSingleton<IPriceStorage, PriceStorage>();

        services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); });

        services.AddOptions<Settings>()
            .Bind(configuration.GetSection(nameof(Settings)));

        services.AddOptions<ChangePriceBehaviourJobConfig>()
            .BindConfiguration(JOB_CONFIGS + ":" + nameof(ChangePriceBehaviourJobConfig));

        services.AddOptions<ChangePriceJobConfig>()
            .BindConfiguration(JOB_CONFIGS + ":" + nameof(ChangePriceJobConfig));

        services.AddQuartz(q => { q.UseMicrosoftDependencyInjectionJobFactory(); });
        services.AddQuartzHostedService(opt => { opt.WaitForJobsToComplete = true; });

        services.AddFluentMigratorCore()
            .ConfigureRunner(r => r
                .AddPostgres11_0()
                .WithGlobalConnectionString(configuration.GetConnectionString("DefaultConnection"))
                .ScanIn(typeof(InitialMigration).Assembly)
                .For.Migrations());

        services
            .AddLogging(l => l.AddFluentMigratorConsole());
    })
    .UseSerilog((context, _, loggerConfiguration) => loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext())
    .Build();

using IServiceScope serviceScope = host.Services.CreateScope();
var provider = serviceScope.ServiceProvider;

var runner = provider.GetRequiredService<IMigrationRunner>();
runner.MigrateUp();

var schedulerFactory = host.Services.GetRequiredService<ISchedulerFactory>();
var scheduler = await schedulerFactory.GetScheduler();

const string STOCK_EXCHANGE_EMULATOR = nameof(STOCK_EXCHANGE_EMULATOR);

var changePriceBehaviourJob = JobBuilder.Create<ChangePriceBehaviourJob>()
    .WithIdentity(nameof(ChangePriceBehaviourJob), STOCK_EXCHANGE_EMULATOR)
    .Build();

var changePriceJob = JobBuilder.Create<ChangePriceJob>()
    .WithIdentity(nameof(ChangePriceJob), STOCK_EXCHANGE_EMULATOR)
    .Build();

var optionsChangePriceBehaviourJobConfig = provider.GetRequiredService<IOptions<ChangePriceBehaviourJobConfig>>();

var changePriceBehaviourJobTrigger = TriggerBuilder.Create()
    .WithIdentity(nameof(changePriceBehaviourJob) + "trigger", STOCK_EXCHANGE_EMULATOR)
    .StartNow()
    .WithSimpleSchedule(x => x
        .WithIntervalInSeconds(optionsChangePriceBehaviourJobConfig.Value.IntervalInSeconds)
        .RepeatForever())
    .Build();

var optionsChangePriceJobConfig = provider.GetRequiredService<IOptions<ChangePriceJobConfig>>();

var changePriceJobTrigger = TriggerBuilder.Create()
    .WithIdentity(nameof(changePriceJob) + "trigger", STOCK_EXCHANGE_EMULATOR)
    .StartNow()
    .WithSimpleSchedule(x => x
        .WithIntervalInSeconds(optionsChangePriceJobConfig.Value.IntervalInSeconds)
        .RepeatForever())
    .Build();

await scheduler.ScheduleJob(changePriceBehaviourJob, changePriceBehaviourJobTrigger);
await scheduler.ScheduleJob(changePriceJob, changePriceJobTrigger);

await host.RunAsync();
