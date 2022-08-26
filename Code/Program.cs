using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, configuration) =>
    {
        configuration.Sources.Clear();
        var env = hostingContext.HostingEnvironment;
        configuration
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        IConfigurationRoot configurationRoot = configuration.Build();
        var options =
            configurationRoot.GetSection(nameof(Settings))
                             .Get<Settings>();
    })
    .ConfigureServices((context, services) =>
    {
        var configurationRoot = context.Configuration;
        //services.AddSingleton<IFillDataService, FillDataService>();

        services.AddOptions<Settings>()
            .Bind(configurationRoot.GetSection(nameof(Settings)));

    })
    .Build();

using IServiceScope serviceScope = host.Services.CreateScope();
var provider = serviceScope.ServiceProvider;
//var fService = provider.GetRequiredService<IFillDataService>();
//await fService.FillDb();
