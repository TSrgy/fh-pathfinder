using FHPathfinder.RecognitionService.Data;
using OpenCvSharp;

namespace FHPathfinder.RecognitionService;

internal static class Helpers
{
    public static IEnumerable<StorageItemType> AllStorageItemTypes => Enum.GetValues<StorageItemType>();

    public static void ShowMat(Mat mat)
    {
        ShowMat(mat, _ => { });
    }

    public static void ShowMat(Mat mat, Action<Mat> action)
    {
        using var output = new Mat();
        mat.CopyTo(output);
        action(output);
        Cv2.ImShow("output", output);
        Cv2.WaitKey();
    }
}