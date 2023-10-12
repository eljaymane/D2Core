using System;
using D2Core.core;
using D2Core.core.network.events;
using D2Core.infrastructure.networking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace D2App
{
	public static class Services
	{
		private static IServiceProvider serviceProvider;
		public static void ConfigureServices()
		{
			var services = new ServiceCollection();
			services.AddLogging();
			services.AddSingleton<ProtocolEventBus>();
			services.AddSingleton<MessageFactory>();
			services.AddTransient<DofusNetworkSniffer>();
			serviceProvider = services.BuildServiceProvider();
			configureLogging();
		}

		//public static IServiceProvider ConfigureLogging()

		public static T Resolve<T>() => serviceProvider.GetService<T>();

		private static void configureLogging()
		{
			using (var loggerFactory = LoggerFactory.Create(builder =>
			{
				builder
					.AddFilter("Microsoft", LogLevel.Warning)
					.AddFilter("System", LogLevel.Warning)
					.AddConsole();
			}));
		}
    }
}

