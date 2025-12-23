using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

namespace Radiotech
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
#if WINDOWS
            // builder.ConfigureLifecycleEvents(events =>
            // {
            //     events.AddWindows(wndLifeCycleBuilder =>
            //     {
            //         wndLifeCycleBuilder.OnWindowCreated(window =>
            //         {
            //             IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            //         });
            //     });
            // });
#endif

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
