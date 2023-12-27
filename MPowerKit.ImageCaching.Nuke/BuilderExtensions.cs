using Microsoft.Maui.Hosting;

namespace MPowerKit.ImageCaching.Nuke;

public static class BuilderExtensions
{
    public static MauiAppBuilder UseMPowerKitNuke(this MauiAppBuilder builder)
    {
#if IOS || MACCATALYST
        builder.ConfigureImageSources(static services =>
        {
            services.AddService<IFileImageSource, NukeFileImageSourceService>();

            services.AddService<IUriImageSource, NukeUriImageSourceService>();

            services.AddService<FileImageSource, NukeFileImageSourceService>();

            services.AddService<UriImageSource, NukeUriImageSourceService>();
        });
#endif

        return builder;
    }
}