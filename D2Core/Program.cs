//// See https://aka.ms/new-console-template for more information
//using D2Core.core;
//using D2Core.core.network.events;
//using D2Core.core.network.messages.chat;
//using D2Core.infrastructure.networking;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using PacketDotNet;
//using SharpPcap;
//using System;
//using System.Buffers.Binary;
//using System.Collections.Concurrent;
//using System.Net;
//using System.Text;

//using (var loggerFactory = LoggerFactory.Create(builder =>
//{
//    builder
//        .AddFilter("Microsoft", LogLevel.Warning)
//        .AddFilter("System", LogLevel.Warning)
//        .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
//        .AddConsole();
//})) ;

//var host = Host.CreateDefaultBuilder()
//        .ConfigureAppConfiguration((context, builder) =>
//        {
//            // Add other configuration files...
//            //builder.AddJsonFile("appsettings.local.json", optional: true);
//        })
//        .ConfigureServices((context, services) =>
//        {
//            ConfigureServices(context.Configuration, services);
//        })
//        .ConfigureLogging((_, logging) =>
//        {
//            logging.AddConsole();
//        })
//        .Build();

//var services = host.Services;

////Capture device
//var devices = CaptureDeviceList.Instance;
//var device = devices.Where(device => device.Name == "en1").FirstOrDefault();

////Initializing data store
//var messageFactory = services.GetRequiredService<MessageFactory>();
//messageFactory.readXmlData("/Volumes/Demesure/Dinvoker/scripts_cs/networkMessages.xml");

////Start sniffing
//var sniffer = services.GetRequiredService<DofusNetworkSniffer>();
//sniffer.SetCaptureDevice(device);
//sniffer.StartCapture();

//var t = new Thread(readAction);
//t.Start();

//sniffer.StartProcessing(new CancellationToken(false));

//void readAction()
//{
//    while (true)
//    {
//        var action = Console.ReadLine();
//    }
//}


//void ConfigureServices(IConfiguration configuration,
//    IServiceCollection services)
//{
//    // ...
//    services.AddSingleton<MessageFactory>();
//    services.AddSingleton<ResponseBuilder>();
//    services.AddSingleton<ProtocolEventBus>();
//    services.AddSingleton<DofusNetworkSniffer>();
//}