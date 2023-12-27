using Foundation;

using MPowerKit.NukeProxy;

using UIKit;

namespace MPowerKit.ImageCaching.Nuke;

public static class NukeExtensions
{
    public static Task<UIImage> LoadImageAsync(NSUrl url)
    {
        var tcs = new TaskCompletionSource<UIImage>();

        ImagePipeline.Shared.LoadImageWithUrl(
            url,
            (image, errorMessage) =>
            {
                if (image == null)
                {
                    tcs.SetException(new Exception(errorMessage));
                }
                tcs.SetResult(image);
            });

        return tcs.Task;
    }

    public static void ClearCache()
    {
        ImageCache.Shared.RemoveAll();
    }
}