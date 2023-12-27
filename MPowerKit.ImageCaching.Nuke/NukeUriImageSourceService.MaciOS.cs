using Foundation;

using UIKit;

namespace MPowerKit.ImageCaching.Nuke;

public class NukeUriImageSourceService : ImageSourceService, IImageSourceService<IUriImageSource>
{
    private readonly float _mainScreenScale;

    public NukeUriImageSourceService()
    {
        _mainScreenScale = (float)UIScreen.MainScreen.Scale;
    }

    public override async Task<IImageSourceServiceResult<UIImage>?> GetImageAsync(IImageSource imageSource, float scale = 1, CancellationToken cancellationToken = default)
    {
        // HACK: MAUI does not pass scale to this method and it is always 1
        if (scale == 1) scale = _mainScreenScale;

        IUriImageSource uriImageSource = (IUriImageSource)imageSource;

        var urlString = uriImageSource.Uri.AbsoluteUri;
        if (string.IsNullOrWhiteSpace(urlString)) return null;

        if (cancellationToken.IsCancellationRequested) return null;

        var nsUrl = NSUrl.FromString(urlString);
        if (nsUrl is null) return null;

        UIImage image;
        try
        {
            image = await NukeExtensions.LoadImageAsync(nsUrl);
        }
        catch
        {
            return null;
        }

        if (image is null) return null;

        return new ImageSourceServiceResult(image, image.Dispose);
    }
}