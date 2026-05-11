
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WyomingCrispAsrServer;
using WyomingCrispAsrServer.Models.Options.WyomingAsrServer;
using WyomingCrispAsrServer.Services;
using WyomingProtocol.Extensions;

var hostBuilder = Host.CreateDefaultBuilder(args);

hostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
    config.AddEnvironmentVariables();
});

hostBuilder.ConfigureServices((context, services) =>
{
    // Logging
    services.AddLogging(builder => builder.AddConsole());

    services.AddWyomingProtocol();

    // Wyoming server setup
    services
        .AddOptions<WyomingAsrServerOptions>()
        .Bind(context.Configuration.GetSection(Constants.Configuration.WyomingCrispAsrServer));

    services.AddSingleton<IValidateOptions<WyomingAsrServerOptions>, ValidateWyomingAsrServerOptions>();

    services.AddSingleton<IWyomingAsrServer, WyomingAsrServer>();
    services.AddWyomingEventHandler<WyomingAsrEventHandler>(Constants.WyomingHandlers.ASR);

    // Server startup
    services.AddHostedService<BootstrapBackgroundService>();
});

var host = hostBuilder.Build();

host.Run();
