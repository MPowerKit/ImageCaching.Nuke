using Foundation;

using UIKit;

namespace MPowerKit.ImageCaching.Nuke;

public class NukeFileImageSourceService : ImageSourceService, IImageSourceService<IFileImageSource>
{
    private static readonly Dictionary<float, string> _scaleToDensitySuffix = new()
    {
        { 1, string.Empty },
        { 2, "@2x" },
        { 3, "@3x" },
        { 4, "@4x" },
        { 5, "@5x" },
        { 6, "@6x" },
    };
    private readonly float _mainScreenScale;

    public NukeFileImageSourceService()
    {
        _mainScreenScale = (float)UIScreen.MainScreen.Scale;
    }

    public override async Task<IImageSourceServiceResult<UIImage>?> GetImageAsync(IImageSource imageSource, float scale = 1, CancellationToken cancellationToken = default)
    {
        // HACK: MAUI does not pass scale to this method and it is always 1
        if (scale == 1) scale = _mainScreenScale;

        IFileImageSource fileImageSource = (IFileImageSource)imageSource;

        var fileName = fileImageSource.File;
        if (string.IsNullOrWhiteSpace(fileName)) return null;

        string name = Path.GetFileNameWithoutExtension(fileName);
        if (string.IsNullOrWhiteSpace(name)) return null;

        string nameWithSuffix = $"{name}{_scaleToDensitySuffix[scale]}";
        string filenameWithDensity = fileName.Replace(name, nameWithSuffix);

        if (cancellationToken.IsCancellationRequested) return null;

        NSUrl fileUrl;
        if (File.Exists(filenameWithDensity))
        {
            fileUrl = NSUrl.FromFilename(filenameWithDensity);
        }
        else if (File.Exists(fileName))
        {
            fileUrl = NSUrl.FromFilename(fileName);
        }
        else
        {
            var imageFromBundle = UIImage.FromBundle(fileName);
            if (imageFromBundle is null) return null;

            return new ImageSourceServiceResult(imageFromBundle, imageFromBundle.Dispose);
        }

        if (cancellationToken.IsCancellationRequested) return null;

        UIImage image;
        try
        {
            image = await NukeExtensions.LoadImageAsync(fileUrl);
        }
        catch
        {
            return null;
        }

        if (image is null) return null;

        return new ImageSourceServiceResult(image, image.Dispose);
    }
}