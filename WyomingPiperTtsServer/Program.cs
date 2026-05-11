using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WyomingPiperTtsServer;
using WyomingPiperTtsServer.Models.Options;
using WyomingPiperTtsServer.Services;
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

    // Piper setup
    services.AddSingleton<IPiperProvider, PiperProvider>();

    services
        .AddOptions<PiperOptions>()
        .Bind(context.Configuration.GetSection(Constants.Configuration.Piper));

    // Wyoming server setup
    services
        .AddOptions<WyomingPiperTtsServerOptions>()
        .Bind(context.Configuration.GetSection(Constants.Configuration.WyomingPiperTtsServer));

    services.AddSingleton<IValidateOptions<WyomingPiperTtsServerOptions>, ValidateWyomingPiperTtsServerOptions>();

    services.AddSingleton<IWyomingAsrTtsServer, WyomingAsrTtsServer>();
    services.AddWyomingEventHandler<WyomingTtsEventHandler>(Constants.WyomingHandlers.TTS);

    // Server startup
    services.AddHostedService<BootstrapBackgroundService>();
});

var host = hostBuilder.Build();

host.Run();