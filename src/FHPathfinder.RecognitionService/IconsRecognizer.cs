using System.Reflection;
using FHPathfinder.RecognitionService.Data;
using Microsoft.Extensions.FileProviders;
using OpenCvSharp;

namespace FHPathfinder.RecognitionService;

public class IconsRecognizer : IDisposable
{
    private readonly DebugOptions _debugOptions;
    private readonly IReadOnlyDictionary<StorageItemType, Mat> _icons;
    private readonly ResourcesTracker _resourcesTracker;

    public IconsRecognizer(DebugOptions debugOptions)
    {
        _debugOptions = debugOptions;
        _resourcesTracker = new ResourcesTracker();

        var icons = new Dictionary<StorageItemType, Mat>();
        var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());

        foreach (var itemType in Helpers.AllStorageItemTypes)
        {
            using (var reader = embeddedProvider.GetFileInfo($"EmbeddedResources/Icons/{itemType}.png").CreateReadStream())
            {
                var mat = _resourcesTracker.T(Mat.FromStream(reader, ImreadModes.Unchanged));
                icons.Add(itemType, mat);
            }
        }

        _icons = icons;
    }

    public IReadOnlyDictionary<StorageItemType, Rect> RecognizeAllIcons(Mat image)
    {
        var recognizedIcons = new Dictionary<StorageItemType, Rect>();
        foreach (var type in Helpers.AllStorageItemTypes)
        {
            var recIcon = RecognizeIcon(type, image);
            if (recIcon.HasValue)
            {
                recognizedIcons.Add(type, recIcon.Value);
            }
        }

        if (_debugOptions.HasFlag(DebugOptions.ShowAllIcons))
        {
            Helpers.ShowMat(image, x =>
            {
                foreach (var rect in recognizedIcons)
                {
                    x.Rectangle(rect.Value, Scalar.LimeGreen, 1, LineTypes.Link4);
                }
            });
        }

        return recognizedIcons;
    }
    
    public Rect? RecognizeIcon(StorageItemType storageItemType, Mat image)
    {
        const double threshold = 0.8;
        var icon = _icons[storageItemType];

        using var matched = image.MatchTemplate(icon, TemplateMatchModes.CCoeffNormed);
        matched.MinMaxLoc(out var minVal, out var maxVal, out var minLoc, out var maxLoc);
        if (maxVal >= threshold)
        {
            var rect = new Rect(maxLoc, icon.Size());

            if (_debugOptions.HasFlag(DebugOptions.ShowEveryIcons))
            {
                Helpers.ShowMat(image, x =>
                {
                    x.Rectangle(rect, Scalar.LimeGreen, 1, LineTypes.Link4);
                });
            }

            return rect;
        }

        return null;
    }

    private void ReleaseUnmanagedResources()
    {
        _resourcesTracker.Dispose();
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~IconsRecognizer()
    {
        ReleaseUnmanagedResources();
    }
}