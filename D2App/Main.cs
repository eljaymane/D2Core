using D2App;
using D2Core.core;
using D2Core.core.network.events;
using D2Core.infrastructure.networking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
// This is the main entry point of the application.
NSApplication.Init ();
NSApplication.Main (args);
var host = Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration((context, builder) =>
        {
            // Add other configuration files...
            //builder.AddJsonFile("appsettings.local.json", optional: true);
        })
        .ConfigureLogging((_, logging) =>
        {
            //ogging.AddConsole();
        })
        .Build();


