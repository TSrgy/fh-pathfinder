using System.Reflection;
using FHPathfinder.RecognitionService.Data;
using Microsoft.Extensions.FileProviders;
using OpenCvSharp;

namespace FHPathfinder.RecognitionService
{
    public partial class StorageRecognizer
    {
        private readonly DebugOptions _debugOptions;
        private readonly IconsRecognizer _iconsRecognizer;
        private readonly NumberFieldsRecognizer _numberFieldsRecognizer;

        public StorageRecognizer(DebugOptions debugOptions, IconsRecognizer iconsRecognizer, NumberFieldsRecognizer numberFieldsRecognizer)
        {
            _debugOptions = debugOptions;
            _iconsRecognizer = iconsRecognizer;
            _numberFieldsRecognizer = numberFieldsRecognizer;
        }

        public void RecognizeStorage(Mat image)
        {
            var recognizedIcon = _iconsRecognizer.RecognizeAllIcons(image);
            var potentialNumberFieldRects = _numberFieldsRecognizer.RecognizePotentialNumberFields(image);

            var storageItemNumberFields = new Dictionary<StorageItemType, Rect>();
            foreach (var (itemType, iconRect) in recognizedIcon)
            {
                var numberFieldRect = potentialNumberFieldRects
                    .Where(x => x.Contains(new Point(iconRect.X + iconRect.Width * 1.5, iconRect.Y + (iconRect.Height / 2))))
                    .Cast<Rect?>()
                    .FirstOrDefault((Rect?)null);
                if (numberFieldRect.HasValue)
                {
                    storageItemNumberFields.Add(itemType, numberFieldRect.Value);
                }
            }

            if (_debugOptions.HasFlag(DebugOptions.ShowStorage))
            {
                Helpers.ShowMat(image, (x) =>
                {
                    foreach (var (itemType, iconRect) in recognizedIcon)
                    {
                        x.Rectangle(iconRect, Scalar.LimeGreen);
                        if (storageItemNumberFields.ContainsKey(itemType))
                        {
                            x.Rectangle(storageItemNumberFields[itemType], Scalar.Red);
                        }
                    }
                });
            }
        }
    }
}