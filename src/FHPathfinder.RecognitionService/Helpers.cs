using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FHPathfinder.RecognitionService.Data;
using FHPathfinder.RecognitionService.Data.StorageItems;
using OpenCvSharp;

namespace FHPathfinder.RecognitionService;

internal static class Helpers
{
    internal static readonly IReadOnlyDictionary<StorageItemType, Type> AllStorageItemTypes = new Dictionary<StorageItemType, Type>()
    {
        { StorageItemType.CrudeOil, typeof(CrudeOil)},
        { StorageItemType.Diesel, typeof(Diesel)},
        { StorageItemType.Petrol, typeof(Petrol)}
    };

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