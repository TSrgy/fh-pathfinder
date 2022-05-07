using FHPathfinder.App.Data;
using FHPathfinder.App.Interfaces;
using Microsoft.AspNetCore.Components.WebView.Maui;

namespace FHPathfinder.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp(IScreenshotService screenshotService)
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            builder.Services.AddSingleton<WeatherForecastService>();

            builder.Services.AddSingleton(screenshotService);

            return builder.Build();
        }
    }
}