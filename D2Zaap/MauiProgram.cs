using D2Core.infrastructure.networking;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using SharpPcap;

namespace D2Zaap;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<DofusNetworkSniffer>();

		IList<ILiveDevice> device = CaptureDeviceList.Instance;

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
