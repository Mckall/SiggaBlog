using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SiggaBlog.Data;
using SiggaBlog.Services;
using SiggaBlog.ViewModel;
using WebApi.Models;

namespace SiggaBlog;

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

#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Services.AddSingleton<AppDbContext>();
        builder.Services.AddSingleton<IRepository<Post>, PostsRepository>();
        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<PostService>();

        return builder.Build();
    }
}
