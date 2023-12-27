using Foundation;

using MPowerKit.NukeProxy;

using UIKit;

namespace MPowerKit.ImageCaching.Nuke;

public static class NukeExtensions
{
    public static Task<UIImage> LoadImageAsync(NSUrl url, float scale = 1)
    {
        var tcs = new TaskCompletionSource<UIImage>();

        ImagePipeline.Shared.LoadScaledImageWithUrl(
            url,
            scale,
            (image, errorMessage) =>
            {
                if (image == null)
                {
                    tcs.SetException(new Exception(errorMessage.ToString()));
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